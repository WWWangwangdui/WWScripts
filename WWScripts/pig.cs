using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WWEngineCC;

namespace WWScripts
{
    public class Pig
        : WWScriptBase,IPlayer
    {
        WWobject obj;
        WWAnimation ani;
        public PointF now;
        public Point last;
        private Point to;
        PointF toword;
        int v = 2;
        MapCtrl Map = null;
        [XmlIgnore()]
        public Random rand;
        Controllor controllor;
        WWobject player;
        WWScript parent;
        int[] dx = new int[] { 0, 0, 1, -1 };
        int[] dy = new int[] { 1, -1, 0, 0 };
        public int V
        {
            get => v;
            set => v = value;
        }
        public override void WWinit(WWScript pt)
        {
            parent = pt;
            obj = pt.Parent;
        }

        public override void WWkilled()
        {
            
        }

        public override void WWload()
        {
           
        }

        public override void WWsave()
        {
            
        }
        
        public void setpos(Point pt)
        {
            last = pt;
            now.X = pt.X * 50 + 25;
            now.Y = pt.Y * 45 + 22.5f;
            obj.Transform.location = new PointF(now.X - 24, now.Y - 32);
            toword = new PointF(now.X, now.Y);
            to = pt;
            ani = obj.WWaddMod("WWAnimation") as WWAnimation;
            ani.BitName = "怪物下_ani";
        }

        public override void WWupdate()
        {
            if(Map==null)
            {
                WWScript script = WWDirector.WWgetGlobleParam("地图") as WWScript;
                if (script == null) return;
                Map = (MapCtrl)script.Script;
            }
            if (Map == null) return;
            if (controllor == null)
            {
                player = WWDirector.WWgetGlobleParam("炸弹人") as WWobject;
                if (player == null) return;
                controllor = (player.WWgetModule("Controllor") as WWScript).Script as Controllor;
                if (controllor == null) return;
            }
            float dis = v;
            int dir = -1;
            /*
             * 0 右
             * 1 左
             * 2 上
             * 3 下
             */
            if (!now.Equals(toword))
            {
                if (now.X < toword.X)
                {
                    float tmp = Math.Min(toword.X - now.X, dis);
                    now.X += tmp;
                    dis -= tmp;
                    dir = 0;
                }
                else if (now.X > toword.X)
                {
                    float tmp = Math.Min(-toword.X + now.X, dis);
                    now.X -= tmp;
                    dis -= tmp;
                    dir = 1;
                }
                else if (now.Y > toword.Y)
                {
                    float tmp = Math.Min(now.Y - toword.Y, dis);
                    now.Y -= tmp;
                    dis -= tmp;
                    dir = 2;
                }
                else if (now.Y < toword.Y)
                {
                    float tmp = Math.Min(toword.Y - now.Y, dis);
                    now.Y += tmp;
                    dis -= tmp;
                    dir = 3;
                }
            }
            if(now.Equals(toword))
            {
                Map.players[last.Y, last.X] = null;
                Point pttmp = last;
                last = new Point((int)now.X / 50, (int)now.Y / 45);
                Map.players[last.Y, last.X] = this;
                List<Point> list = new List<Point>();
                for (int i = 0; i < 4; i++)
                {
                    if (new Point(last.X + dx[i], last.Y + dy[i]).Equals(pttmp)) continue;
                    if (Map.map[last.Y + dy[i], last.X + dx[i]] == 0 && (Map.players[last.Y + dy[i], last.X + dx[i]] == null || Map.players[last.Y + dy[i], last.X + dx[i]] is Controllor)) 
                    {
                        list.Add(new Point(dx[i], dy[i]));
                    }
                }
                if(list.Count==0)
                {
                    if (Map.map[pttmp.Y,pttmp.X] == 0 && (Map.players[pttmp.Y, pttmp.X] == null || Map.players[pttmp.Y, pttmp.X] is Controllor))
                    {
                        list.Add(new Point(pttmp.X - last.X, pttmp.Y - last.Y));
                    }
                }
                if (list.Count !=0)
                {
                    int sel = rand.Next(0, list.Count);
                    to = new Point(last.X + list[sel].X, last.Y + list[sel].Y);
                    Map.players[to.Y, to.X] = this;
                    toword = new PointF(50 * to.X + 25, 45 * to.Y + 22.5f);
                }
            }
            if(!now.Equals(toword)&&dis!=0.0f)
            {
                if (now.X < toword.X)
                {
                    float tmp = Math.Min(toword.X - now.X, dis);
                    now.X += tmp;
                    dir = 0;
                }
                else if (now.X > toword.X)
                {
                    float tmp = Math.Min(-toword.X + now.X, dis);
                    now.X -= tmp;
                    dir = 1;
                }
                else if (now.Y > toword.Y)
                {
                    float tmp = Math.Min(now.Y - toword.Y, dis);
                    now.Y -= tmp;
                    dir = 2;
                }
                else if (now.Y < toword.Y)
                {
                    float tmp = Math.Min(toword.Y - now.Y, dis);
                    now.Y += tmp;
                    dir = 3;
                }
            }
            string bitname = ani.BitName;
            if (dir == 0) bitname = "怪物右_ani";
            else if (dir == 1) bitname = "怪物左_ani";
            else if (dir == 2) bitname = "怪物上_ani";
            else if (dir == 3) bitname = "怪物下_ani";
            if (bitname != ani.BitName) ani.BitName = bitname;
            obj.Transform.location = new PointF(now.X - 24, now.Y - 64);
            ani.Level = ((int)now.Y / 45) * 5 + 3;
            if(Math.Abs(now.X-Map.player.Transform.location.X)<45&&Math.Abs(now.Y-Map.player.Transform.location.Y)<45)
            {
                controllor.Die();
            }
        }

        public void Die()
        {
            Map.players[last.Y, last.X] = null;
            Map.players[to.Y, to.X] = null;
            parent.activity = false;
            ani.activity = false;
        }
    }
}
