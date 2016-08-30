using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGPoint
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal xOrig { get; set; }
        public decimal yOrig { get; set; }

        public SVGPoint()
        {
            x = decimal.MinValue;
            y = decimal.MinValue;
        }

        public SVGPoint(decimal x, decimal y)
        {
            this.x = x;
            this.y = y;            
        }

        public void Add(SVGPoint p)
        {
            this.x += p.x;
            this.y += p.y;
        }
    }
}
