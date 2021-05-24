using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WWEngineCC;
namespace WWScripts
{
    public class background
        : WWScriptBase
    {
        WWScript parent;
        public override void WWinit(WWScript pt)
        {
            parent = pt;
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
            if (parent == null || parent.Parent == null || parent.Parent.Transform == null) return;
            PointF pointF = parent.Parent.Transform.location;
            Point point = WWkeyIO.WWmousePoint;
            WWEngineCC.WWBitmap bit = parent.Parent.WWgetModule("WWBitmap") as WWEngineCC.WWBitmap;
            if (point.X>=370&&point.X<=600&&point.Y>=240&&point.Y<=290)
            {
                bit.BitName = "选择一";
                if(WWkeyIO.WWgetDown(System.Windows.Forms.Keys.LButton))
                {
                    WWDirector.WWloadScene(1);
                }
            }
            else if(point.X >= 370 && point.X <= 600 && point.Y >= 330 && point.Y <= 380)
            {
                bit.BitName = "选择二";
            }
            else
            {
                bit.BitName = "选择关卡";
            }
            
        }
    }
}
