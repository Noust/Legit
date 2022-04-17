using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Legit
{
    public class ESPPP
    {
        public float X, Z, Y, cabezaX, cabezaZ, cabezaY;

        public int VidaA,dormantt,Teamnum,m_flag,armour;

        public Point bottom, top,bonecabeza,bonecabezatop;



        public Rectangle rect()
        {
            return new Rectangle
            {
                Location = new Point(bottom.X - (bottom.Y - top.Y) / 4, top.Y),
                Size = new Size((bottom.Y - top.Y) / 2, bottom.Y - top.Y)
            };
        }

        public Rectangle healthbar(ESPPP entity)
        {
            return new Rectangle
            {
                Location = new Point(bottom.X - (bottom.Y - top.Y) / 3 + 2, top.Y),
                Size = new Size((bottom.Y - top.Y) / 40, (bottom.Y - top.Y) * (entity.VidaA) / 100)
            };
        }

        public Rectangle ArmourBar(ESPPP entity)
        {
            return new Rectangle
            {
                Location = new Point(bottom.X - (bottom.Y - top.Y) / 5 + (bottom.Y - top.Y) / 2, top.Y),
                Size = new Size((bottom.Y - top.Y) / 40, (bottom.Y - top.Y) * (entity.armour) / 100)
            };
        }
        public Rectangle headrect()
        {
            return new Rectangle
            {
                Location = new Point(bonecabeza.X - (bonecabeza.Y - bonecabezatop.Y) / 2, bonecabezatop.Y + 5),
                Size = new Size((bonecabeza.Y - bonecabezatop.Y) / 1, bonecabeza.Y - bonecabezatop.Y)
            };
        }
    }

    public class ViewMatrix
    {
        public float m11, m12, m13, m14;
        public float m21, m22, m23, m24;
        public float m31, m32, m33, m34;
        public float m41, m42, m43, m44;
    }

}
