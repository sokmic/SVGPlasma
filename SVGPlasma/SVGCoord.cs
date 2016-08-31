using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGCoord: SVGParameter
    {
        public decimal n { get; set; }
        public decimal nOrig { get; set; }

        public SVGCoord()
        {
            n = decimal.MinValue;
        }

        public SVGCoord(decimal n)
        {
            this.n = n;
        }

        public void Add(SVGCoord c)
        {
            this.n += c.n;
        }
    }
}
