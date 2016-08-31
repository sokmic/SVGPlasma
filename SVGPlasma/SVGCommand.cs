using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    public enum SVGCmdType
    {
        Absolute,
        Relative
    }

    class SVGCommand
    {
        public string command { get; set; }
        public List<SVGParameter> pars = new List<SVGParameter>();
        public SVGCmdType type { get; set; }
    }
}
