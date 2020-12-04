using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma.SVGCommands
{
    class SVGCommandClosePath : SVGCommand
    {
        public SVGCommandClosePath(SVGToken t) : base(t) { }
        public override void ParseParameters(SVGTokenStream ts)
        {
            //no parameters
            if(ts.peek().tokType == TokenType.Number)
                throw new Exception("Invalid SVG File.  Unexpected token following Close Path command.");            
        }

        public override Coordinate PositionAfterCommand(Coordinate current, Coordinate start)
        {
            return start;
        }
    }
}
