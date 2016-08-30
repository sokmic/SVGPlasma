using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class GCodeMaterialSettings
    {
        public decimal PierceTime { get; set; } // seconds
        public decimal FeedRate { get; set; } // mm/min
        public GCodeMaterialSettings()
        {

        }
    }
}
