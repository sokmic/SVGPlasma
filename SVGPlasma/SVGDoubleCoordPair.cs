using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGDoubleCoordPair :SVGParameter
    {
        public SVGCoordPair p1 { get; set; }
        public SVGCoordPair p2 { get; set; }

        public SVGDoubleCoordPair(SVGCoordPair cp1, SVGCoordPair cp2)
        {
            p1 = cp1;
            p2 = cp2;
        }

    }
}
