using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WWEngineCC;
namespace WWScripts
{
    public class MapCtrl
        : WWScriptBase
    {
        string path;
        WWobject obj;
        WWScript parent;
        bool inited = false;
        [XmlIgnore()]
        public int[,] map;
        [XmlIgnore()]
        WWEngineCC.WWBitmap[,] objects;
        [XmlIgnore()]
        public IPlayer[,] players;
        [XmlIgnore()]
        public WWobject player;
        int N, M;
        Random rand = new Random();
        public override void WWinit(WWScript pt)
        {
            path = WWDirector.WWProject.SavePath + "01.txt";
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

        public void WWboom(Point pt,int u,int d,int l,int r)
        {
            if(players[pt.Y,pt.X]!=null)
            {
                players[pt.Y, pt.X].Die();
            }
            for(int i=1;i<=u;i++)
            {
                if (players[pt.Y - i, pt.X] != null)
                {
                    players[pt.Y - i, pt.X].Die();
                }
            }
            for (int i = 1; i <= d; i++)
            {
                if (players[pt.Y + i, pt.X] != null)
                {
                    players[pt.Y + i, pt.X].Die();
                }
            }
            for (int i = 1; i <= l; i++)
            {
                if (players[pt.Y, pt.X - i] != null)
                {
                    players[pt.Y, pt.X - i].Die();
                }
            }
            for (int i = 1; i <= r; i++)
            {
                if (players[pt.Y, pt.X + i] != null)
                {
                    players[pt.Y, pt.X + i].Die();
                }
            }
        }

        public void WWdelblock(int i,int j)
        {
            map[i,j] = 0;
            objects[i, j].BitName = "floor01";
            objects[i, j].LeftTop = new PointF(1, 22);
        }

        public override void WWupdate()
        {
            if (obj == null) obj = parent.Parent;
            if (obj == null) return;
            if(player==null)
            {
                player = WWDirector.WWgetGlobleParam("炸弹人") as WWobject;
                if (player == null) return;
            }
            if (!inited)
            {
                try
                {
                    StreamReader reader = new StreamReader(path);
                    int n, m;
                    string str = reader.ReadLine();
                    string[] buffer = str.Split();
                    n = Convert.ToInt32(buffer[0]);
                    m = Convert.ToInt32(buffer[1]);
                    map = new int[n, m];
                    objects = new WWEngineCC.WWBitmap[n, m];
                    players = new IPlayer[n, m];
                    N = n;
                    M = m;
                    for(int i=0;i<n;i++)
                    {
                        str = reader.ReadLine();
                        buffer = str.Split();
                        for(int j=0;j<m;j++)
                        {
                            map[i,j] = Convert.ToInt32(buffer[j]);
                        }
                    }
                    for (int i = 0; i < n; i++)
                    {
                        str = reader.ReadLine();
                        buffer = str.Split();
                        string name = "monstor";
                        int cnt = 0;
                        for (int j = 0; j < m; j++)
                        {
                            if(Convert.ToInt32(buffer[j])==1)
                            {
                                player.activity = true;
                                player.Transform.location = new PointF(50 * j + 25, 45 * i + 22.5f);
                            }
                            else if(Convert.ToInt32(buffer[j]) == 2)
                            {
                                WWobject tmp = WWDirector.WWaddObj(name + cnt.ToString());
                                tmp.WWaddScript("Pig");
                                Pig pig = (tmp.WWgetModule("Pig") as WWScript).Script as Pig;
                                pig.rand = new Random(rand.Next());
                                pig.setpos(new Point(j, i));
                            }
                        }
                    }
                    int x = 0, y = 0;
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < m; j++)
                        {
                            WWobject cur = obj.WWaddObj(i.ToString() + "," + j.ToString());
                            WWTransform trans;
                            if (cur.Transform != null) trans = cur.Transform;
                            else trans = cur.WWaddMod("WWTransform") as WWTransform;
                            WWEngineCC.WWBitmap bit = cur.WWaddMod("WWBitmap") as WWEngineCC.WWBitmap;
                            objects[i, j] = bit;
                            switch (map[i, j])
                            {
                                case 0: bit.BitName = "floor01";
                                    trans.location = new System.Drawing.PointF(x, y);
                                    bit.Level = 0;
                                    break;
                                case 1: bit.BitName = "block01";
                                    trans.location = new System.Drawing.PointF(x - 1, y - 22);
                                    bit.Level = 1;
                                    break;
                                case 2:
                                    bit.BitName = "wall01";
                                    trans.location = new PointF(x - 0.5f, y - 8);
                                    bit.Level = 1;
                                    break;
                                default:
                                    break;
                            }
                            x += 50;
                        }
                        y += 45;
                        x = 0;
                    }
                    inited = true;
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < M; j++)
                        {
                            if (map[i, j] == 0)
                                objects[i, j].Level = 5 * i;
                            else objects[i, j].Level = 5 * i + 1;
                        }
                    }
                    WWDirector.WWaddGlobleParam("地图", parent);
                }
                catch
                {

                }
            }
            else
            {

            }
                
        }
        /*
         * 0 floor01 
         * 1 block01
         * 2 wall
         * 3 boom
         */
    }
}
