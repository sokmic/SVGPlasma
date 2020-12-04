using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    public class GCodeMaterialSettings
    {
        public string MaterialName { get; set; }
        public decimal PierceTime { get; set; } // seconds
        public decimal FeedRate { get; set; } // mm/min
        public GCodeMaterialSettings()
        {
            MaterialName = "";
            PierceTime = 0;
            FeedRate = 0;
        }
    }
}
