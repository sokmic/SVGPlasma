using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    public class GCodeMachineSettings
    {
        public string MachineName { get; set; }
        public string BeginCode { get; set; }
        public string EndCode { get; set; }
        public string SpindleOnCode { get; set; }
        public string SpindleOffCode { get; set; }
        public decimal CutWidth { get; set; } // mm

        public GCodeMachineSettings()
        {
            MachineName = "";
            BeginCode = "; begin g-code";
            EndCode = "; end g-code";
            SpindleOnCode = "M3 S255";
            SpindleOffCode = "M5";
            CutWidth = 0;
        }        
    }
}
