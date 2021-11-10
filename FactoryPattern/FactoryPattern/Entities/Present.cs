using FactoryPattern.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPattern.Entities
{
    public class Present : Toy
    {
        public SolidBrush ribbon { get; set; }
        public SolidBrush box { get; set; }

        public Present(Color Ribbon, Color Box)
        {
            ribbon = new SolidBrush(Ribbon);
            box = new SolidBrush(Box);
        }
        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(box, 0, 0, Width, Height);
        }
    }
}

