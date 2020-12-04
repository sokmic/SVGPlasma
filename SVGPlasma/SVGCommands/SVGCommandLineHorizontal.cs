using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma.SVGCommands
{
    class SVGCommandLineHorizontal : SVGCommand
    {
        public SVGCommandLineHorizontal(SVGToken t) : base(t) { }

        public decimal x { get; set; }
        public override void ParseParameters(SVGTokenStream ts)
        {
            //(x)
            //draw a horizontal line to position x
            SVGToken xt = ts.getToken();
            if (xt.tokType != TokenType.Number)
                throw new Exception("Invalid SVG File.  Unexpected token in path. '" + xt.value + "'");
            x = decimal.Parse(xt.value);
        }

        public override Coordinate PositionAfterCommand(Coordinate current, Coordinate start)
        {
            if (type == SVGCmdType.Absolute)
                return new Coordinate(x, current.Y);
            else
                return current + new Coordinate(x, 0);
        }
    }
}
