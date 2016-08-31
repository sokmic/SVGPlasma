using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGEArcArgs :SVGParameter
    {
        public decimal n1 { get; set; }
        public decimal n2 { get; set; }
        public decimal n3 { get; set; }
        public bool flag1 { get; set; }
        public bool flag2 { get; set; }
        public SVGCoordPair p { get; set; }


        public SVGEArcArgs(decimal d1, decimal d2, decimal d3, bool b1, bool b2, SVGCoordPair pair)
        {
            n1 = d1;
            n2 = d2;
            n3 = d3;
            flag1 = b1;
            flag2 = b2;
            p = pair;
        }

    }
}
