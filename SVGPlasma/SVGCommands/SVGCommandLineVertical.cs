using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma.SVGCommands
{
    class SVGCommandLineVertical : SVGCommand
    {
        public SVGCommandLineVertical(SVGToken t) : base(t) { }

        public decimal y { get; set; }
        public override void ParseParameters(SVGTokenStream ts)
        {
            //(y)
            //draw a vertical line to position y            
            SVGToken yt = ts.getToken();
            if (yt.tokType != TokenType.Number)
                throw new Exception("Invalid SVG File.  Unexpected token in path. '" + yt.value + "'");
            y = decimal.Parse(yt.value);
        }

        public override Coordinate PositionAfterCommand(Coordinate current, Coordinate start)
        {
            if (type == SVGCmdType.Absolute)
                return new Coordinate(current.X, y);
            else
                return current + new Coordinate(0, y);
        }
    }
}
