using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWEngineCC;

namespace WWScripts
{
    public interface IPlayer
    {
        void Die();
    }
    public class Controllor
           : WWScriptBase,IPlayer
    {
        
        WWTransform trans = null;
        WWAnimation ani = null;
        WWScript module;
        int cstlr = 0, cstud = 0;
        float v = 1;
        private MapCtrl Map;
        private bool die = false;
        public float V
        {
            get => v;
            set => v = value;
        }
        public float VVvVasfas
        {
            get => v * v;
        }
        public override void WWinit(WWScript pt)
        {
            module = pt;
            WWobject obj = pt.Parent;
            if (obj == null) return;
            WWDirector.WWaddGlobleParam("炸弹人", obj);
            trans = obj.Transform;
            ani = obj.WWgetModule("WWAnimation") as WWAnimation;
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

        private Point pos(PointF pt)
        {
            Point ans = new Point();
            ans.X = ((int)pt.X) / 50;
            ans.Y = ((int)pt.Y) / 45;
            ans.X = Math.Max(0, Math.Min(ans.X, Map.map.GetLength(1)));
            ans.Y = Math.Max(0, Math.Min(ans.Y, Map.map.GetLength(0)));
            return ans;
        }

        private Point moveCheck(PointF pt, int lr, int ud)
        {
            if(lr==0&&ud==0)return new Point(-111, -111);
            if (Map == null) return new Point(-111, -111);
            PointF tmp = new PointF(pt.X, pt.Y);
            tmp.X += lr * (v + 25);
            tmp.Y += ud * (v + 22.5f);
            Point cell = pos(tmp);
            if (cell.Equals(pos(pt)))return cell;
            if (Map.map[cell.Y, cell.X] != 0)
            {
                return new Point(-111, -111);
            }
            return cell;
        }

        private Point gettoword(int lr,int ud)
        {
            if (lr == 0 && ud == 0) return new Point(-111, -111);
            if (Map == null) return new Point(-111, -111);
            PointF pt = trans.globleLocation;
            Point cell = pos(pt);
            if (lr == 1)
            {
                Point toword;
                if (pt.Y % 45 <= 15)
                {
                    toword = new Point(cell.X + 1, cell.Y - 1);
                    if (Map.map[cell.Y - 1, cell.X] == 0 && Map.map[toword.Y, toword.X] == 0)
                        return toword;
                }
                else if (pt.Y % 45 >= 30)
                {
                    toword = new Point(cell.X + 1, cell.Y + 1);
                    if (Map.map[cell.Y + 1, cell.X] == 0 && Map.map[toword.Y, toword.X] == 0)
                        return toword;
                }
                else
                {
                    return new Point(-111, -111);
                }
            }
            else if (lr == -1) 
            {
                Point toword;
                if (pt.Y % 45 <= 15)
                {
                    toword = new Point(cell.X - 1, cell.Y - 1);
                    if (Map.map[cell.Y - 1, cell.X] == 0 && Map.map[toword.Y, toword.X] == 0)
                        return new Point(cell.X, cell.Y - 1);
                }
                else if (pt.Y % 45 >= 30)
                {
                    toword = new Point(cell.X - 1, cell.Y + 1);
                    if (Map.map[cell.Y + 1, cell.X] == 0 && Map.map[toword.Y, toword.X] == 0)
                        return new Point(cell.X, cell.Y + 1);
                }
                else
                {
                    return new Point(-111, -111);
                }
            }
            else if (ud == 1)
            {
                Point toword;
                if (pt.X % 50 <= 15)
                {
                    toword = new Point(cell.X - 1, cell.Y + 1);
                    if (Map.map[cell.Y, cell.X - 1] == 0 && Map.map[toword.Y, toword.X] == 0)
                        return new Point(cell.X - 1, cell.Y);
                }
                else if (pt.X % 50 >= 30)
                {
                    toword = new Point(cell.X + 1, cell.Y + 1);
                    if (Map.map[cell.Y, cell.X + 1] == 0 && Map.map[toword.Y, toword.X] == 0)
                        return new Point(cell.X + 1, cell.Y);
                }
                else
                {
                    return new Point(-111, -111);
                }
            }
            else if (ud == -1)
            {
                Point toword;
                if (pt.X % 50 <= 15)
                {
                    toword = new Point(cell.X - 1, cell.Y - 1);
                    if (Map.map[cell.Y, cell.X - 1] == 0 && Map.map[toword.Y, toword.X] == 0)
                        return toword;
                }
                else if (pt.X % 50 >= 30)
                {
                    toword = new Point(cell.X + 1, cell.Y - 1);
                    if (Map.map[cell.Y, cell.X + 1] == 0 && Map.map[toword.Y, toword.X] == 0)
                        return toword;
                }
                else
                {
                    return new Point(-111, -111);
                }
            }
            return new Point(-111, -111);
        }

        private PointF getv(int lr, int ud, Point toword)
        {
            PointF res = new PointF();
            PointF pt = trans.globleLocation;
            PointF to = new PointF(toword.X * 50 + 25, toword.Y * 45 + 22.5f);
            if (pt.X == to.X)
            {
                if (to.Y > pt.Y)
                {
                    res.Y = v;
                }
                else
                {
                    res.Y = -v;
                }
            }
            else if (pt.Y == to.Y)
            {
                if (to.X > pt.X)
                {
                    res.X = v;
                }
                else
                {
                    res.X = -v;
                }
            }
            else
            {
                if (to.X - pt.X >= 0 && to.X - pt.X <= 25)
                {
                    res.X = Math.Min(v, to.X - pt.X);
                    if (to.Y > pt.Y)
                    {
                        res.Y = v - res.X;
                    }
                    else
                    {
                        res.Y = -(v - res.X);
                    }
                }
                else if (to.X - pt.X <= 0 && to.X - pt.X >= -25)
                {
                    res.X = Math.Max(-v, to.X - pt.X);
                    if (to.Y > pt.Y)
                    {
                        res.Y = v + res.X;
                    }
                    else
                    {
                        res.Y = -(v + res.X);
                    }
                }
                else if (to.Y - pt.Y >= 0 && to.Y - pt.Y <= 22.5)
                {
                    res.Y = Math.Min(v, to.Y - pt.Y);
                    if (to.X > pt.X)
                    {
                        res.X = v - res.Y;
                    }
                    else
                    {
                        res.X = -(v - res.Y);
                    }
                }
                else if (to.Y - pt.Y <= 0 && to.Y - pt.Y >= -22.5)
                {
                    res.Y = Math.Max(-v, to.Y - pt.Y);
                    if (to.X > pt.X)
                    {
                        res.X = v + res.Y;
                    }
                    else
                    {
                        res.X = -(v + res.Y);
                    }
                }
                else
                {
                    res.X = res.Y = 0;
                }
            }
            return res;
        }


        public override void WWupdate()
        {
            #region//move
            if (Map == null)
            {
                WWScript script = WWDirector.WWgetGlobleParam("地图") as WWScript;
                if (script == null) return;
                Map = (MapCtrl)script.Script;
            }
            if (Map == null) return;
            if (trans == null || ani == null)
            {
                WWobject obj = module.Parent;
                if (obj == null) return;
                WWDirector.WWaddGlobleParam("主角", obj);
                trans = obj.Transform;
                ani = obj.WWgetModule("WWAnimation") as WWAnimation;
            }
            if (trans == null || ani == null)
            {
                return;
            }
            int lr = 0, ud = 0;
            if (!die)
            {
                if (WWkeyIO.WWgetDown(System.Windows.Forms.Keys.A)) lr--;
                if (WWkeyIO.WWgetDown(System.Windows.Forms.Keys.D)) lr++;
                if (lr == 0)
                {
                    if (WWkeyIO.WWgetDown(System.Windows.Forms.Keys.W)) ud--;
                    if (WWkeyIO.WWgetDown(System.Windows.Forms.Keys.S)) ud++;
                }
                if (lr == -1)
                {
                    if (lr != cstlr)
                        ani.BitName = "向左走_ani";
                }
                else if (lr == 1)
                {
                    if (lr != cstlr)
                        ani.BitName = "向右走_ani";
                }
                else if (ud == -1)
                {
                    if (ud != cstud)
                        ani.BitName = "向上走_ani";
                }
                else if (ud == 1)
                {
                    if (ud != cstud)
                        ani.BitName = "向下走_ani";
                }
                cstlr = lr;
                cstud = ud;
                if (lr == 0 && ud == 0)
                {
                    ani.Flush();
                    ani.Enable = false;
                }
                else
                {
                    if (ani.Enable == false)
                    {
                        ani.Enable = true;
                        ani.Flush();
                    }
                }
                Point pt = pos(trans.location);
                if (WWkeyIO.WWgetDown(System.Windows.Forms.Keys.Space))
                {
                    if (Map.map[pt.Y, pt.X] == 0)
                    {
                        Map.map[pt.Y, pt.X] = 3;
                        WWobject obj = WWDirector.WWaddObj();
                        obj.Transform.location = new PointF(pt.X * 50, pt.Y * 45);

                        WWEngineCC.WWAnimation bit = obj.WWaddMod("WWAnimation") as WWEngineCC.WWAnimation;
                        bit.BitName = "boom_ani";
                        obj.WWaddScript("boom");
                    }
                }
                Map.players[pt.Y, pt.X] = null;
                Point toword = moveCheck(trans.location, lr, ud);
                if (toword.X == -111) toword = gettoword(lr, ud);
                if (toword.X != -111)
                {
                    PointF _v = getv(lr, ud, toword);
                    trans.location = new System.Drawing.PointF(trans.location.X + _v.X, trans.location.Y + _v.Y);
                }
                #endregion
                pt = pos(trans.location);
                Map.players[pt.Y, pt.X] = this;
                ani.Level = pt.Y * 5 + 3;
            }
            else
            {
                if (ani.CurFrame == 6)
                {
                    module.Parent.activity = false;
                    ani.Enable = false;
                    WWDirector.WWloadScene(0);
                }
            }
        }
        public void Die()
        {
            if (!die)
            {
                die = true;
                ani.BitName = "die";
                ani.Enable = true;
            }
        }
    }
}
