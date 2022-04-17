using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Legit
{
    public class Jugador
    {
        public int Vida { get; set; }
        public float X { get; set; }

        public float Z { get; set; }

        public float Y { get; set; }

        public int dormant { get; set; }

        public double Magnitud { get; set; }

        public int Teamnum { get; set; }
        

        public Point BoneCabeza;
    }
}
