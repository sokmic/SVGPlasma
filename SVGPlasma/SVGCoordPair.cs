using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGCoordPair: SVGParameter
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal xOrig { get; set; }
        public decimal yOrig { get; set; }

        public SVGCoordPair()
        {
            x = decimal.MinValue;
            y = decimal.MinValue;
        }

        public SVGCoordPair(decimal x, decimal y)
        {
            this.x = x;
            this.y = y;            
        }

        public SVGCoordPair(double x, double y)
        {
            this.x = (decimal)x;
            this.y = (decimal)y;
        }

        public void Add(SVGCoordPair p)
        {
            this.x += p.x;
            this.y += p.y;
        }
    }
}
