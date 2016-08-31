using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGTripleCoordPair :SVGParameter
    {
        public SVGCoordPair p1 { get; set; }
        public SVGCoordPair p2 { get; set; }
        public SVGCoordPair p3 { get; set; }

        public SVGTripleCoordPair(SVGCoordPair cp1, SVGCoordPair cp2, SVGCoordPair cp3)
        {
            p1 = cp1;
            p2 = cp2;
            p3 = cp3;
        }

    }
}
