using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWEngineCC;
namespace WWScripts
{
    public class boom : WWScriptBase
    {
        private MapCtrl Map;
        private int power=1;
        private double boomTimer;
        private double delteTime=3000;
        private WWScript boomparent;
        private WWobject boomnow;
        private Point boomPoint;
        private bool isboom = false;
        public int Power
        {
            get
            {
                return power;
            }
            set
            {
                power = value;
            }
        }
        public double DelteTime
        {
            get => delteTime;
            set => delteTime = value;
        }
        public override void WWinit(WWScript pt)
        {
            boomTimer = WWTime.now + delteTime;
            boomparent = pt;
            if(boomparent!=null)
            {
                boomnow = boomparent.Parent;
            }
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

        public override void WWupdate()
        {
            if (boomnow == null)
            {
                boomnow = boomparent.Parent;
            }
            if (boomnow == null) return;
            boomPoint.X = (int)boomnow.Transform.globleLocation.X / 50;
            boomPoint.Y = (int)boomnow.Transform.globleLocation.Y / 45;
            (boomnow.WWgetModule("WWAnimation") as WWAnimation).Level = boomPoint.Y * 5 + 3;
            if (Map == null)
            {
                WWScript script = WWDirector.WWgetGlobleParam("地图") as WWScript;
                if (script == null) return;
                Map = (MapCtrl)script.Script;
                if(Map!=null)
                {
                    Map.map[boomPoint.Y,boomPoint.X] = 3;
                }
            }
            if (Map == null) return;
            if (!isboom)
            {
                if (WWTime.now >= boomTimer)
                {
                    boomTimer = WWTime.now + 500;
                    isboom = true;
                    int u = 0, d = 0, l = 0, r = 0;
                    #region 
                    for (int i = 1; i <= power; i++)
                    {
                        if (Map.map[boomPoint.Y - i, boomPoint.X] == 0)
                        {
                            u++;
                        }
                        else if (Map.map[boomPoint.Y - i, boomPoint.X] == 1)
                        {
                            u++;
                            Map.WWdelblock(boomPoint.Y - i, boomPoint.X);
                            break;
                        }
                        else break;
                    }
                    for (int i = 1; i <= power; i++)
                    {
                        if (Map.map[boomPoint.Y + i, boomPoint.X] == 0)
                        {
                            d++;
                        }
                        else if (Map.map[boomPoint.Y + i, boomPoint.X] == 1)
                        {
                            d++;
                            Map.WWdelblock(boomPoint.Y + i, boomPoint.X);
                            break;
                        }
                        else break;
                    }
                    for (int i = 1; i <= power; i++)
                    {
                        if (Map.map[boomPoint.Y, boomPoint.X - i] == 0)
                        {
                            l++;
                        }
                        else if (Map.map[boomPoint.Y, boomPoint.X - i] == 1)
                        {
                            l++;
                            Map.WWdelblock(boomPoint.Y, boomPoint.X - i);
                            break;
                        }
                        else break;
                    }
                    for (int i = 1; i <= power; i++)
                    {
                        if (Map.map[boomPoint.Y, boomPoint.X + i] == 0)
                        {
                            r++;
                        }
                        else if (Map.map[boomPoint.Y, boomPoint.X + i] == 1)
                        {
                            r++;
                            Map.WWdelblock(boomPoint.Y, boomPoint.X + i);
                            break;
                        }
                        else break;
                    }

                    #endregion
                    #region
                    (boomnow.WWgetModule("WWAnimation") as WWAnimation).BitName = ("boommid");
                    (boomnow.WWgetModule("WWAnimation") as WWAnimation).Level = 1000;
                    for (int i = 1; i <= l; i++)
                    {
                        WWobject obj = boomnow.WWaddObj("l" + i.ToString());
                        obj.Transform.location = new PointF(-i * 50, 0);
                        WWEngineCC.WWBitmap bit = obj.WWaddMod("WWBitmap") as WWEngineCC.WWBitmap;
                        if (i < l) bit.BitName = "boomlr";
                        else bit.BitName = "lend";
                        bit.Level = 1000;
                    }
                    for (int i = 1; i <= r; i++)
                    {
                        WWobject obj = boomnow.WWaddObj("r" + i.ToString());
                        obj.Transform.location = new PointF(i * 50, 0);
                        WWEngineCC.WWBitmap bit = obj.WWaddMod("WWBitmap") as WWEngineCC.WWBitmap;
                        if (i < r) bit.BitName = "boomlr";
                        else bit.BitName = "rend";
                        bit.Level = 1000;
                    }
                    for (int i = 1; i <= u; i++)
                    {
                        WWobject obj = boomnow.WWaddObj("u" + i.ToString());
                        obj.Transform.location = new PointF(0, -i * 50);
                        WWEngineCC.WWBitmap bit = obj.WWaddMod("WWBitmap") as WWEngineCC.WWBitmap;
                        if (i < u) bit.BitName = "boomud";
                        else
                        {
                            obj.Transform.location = new PointF(0, -i * 50 + 5);
                            bit.BitName = "uend";
                        }
                        bit.Level = 1000;
                    }
                    for (int i = 1; i <= d; i++)
                    {
                        WWobject obj = boomnow.WWaddObj("d" + i.ToString());
                        obj.Transform.location = new PointF(0, i * 50);
                        WWEngineCC.WWBitmap bit = obj.WWaddMod("WWBitmap") as WWEngineCC.WWBitmap;
                        if (i < d) bit.BitName = "boomud";
                        else
                        {
                            obj.Transform.location = new PointF(0, i * 50 - 5);
                            bit.BitName = "dend";
                        }
                        bit.Level = 1000;
                    }
                    #endregion
                    Map.WWboom(boomPoint, u, d, l, r);
                    /* boommid 爆炸中心 设置成一个动画
                     * lend 左边尽头
                     * rend 右边尽头
                     * uend 上尽头
                     * dend 下尽头
                     * boomud 上下
                     * boomlr 左右 
                     */
                }
            }
            else
            {
                if (WWTime.now >= boomTimer)
                {
                    Map.map[boomPoint.Y, boomPoint.X] = 0;
                    WWDirector.WWdelObj(boomnow.ID);
                }
                else
                {
                    (boomnow.WWgetModule("WWAnimation") as WWAnimation).Level = 1000;
                }
            }

        }

        private void onboom()
        {

        }

    }
}
