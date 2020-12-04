using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma.SVGCommands
{
    class SVGCommandLineTo : SVGCommand
    {
        public SVGCommandLineTo(SVGToken t) : base(t) { }

        public decimal x { get; set; }
        public decimal y { get; set; }
        public override void ParseParameters(SVGTokenStream ts)
        {
            //(x y)
            //draw a line to position x, y
            SVGToken xt = ts.getToken();
            if (xt.tokType != TokenType.Number)
                throw new Exception("Invalid SVG File.  Unexpected token in path. '" + xt.value + "'");
            x = decimal.Parse(xt.value);

            SVGToken yt = ts.getToken();
            if (yt.tokType != TokenType.Number)
                throw new Exception("Invalid SVG File.  Unexpected token in path. '" + yt.value + "'");
            y = decimal.Parse(yt.value);

        }

        public override Coordinate PositionAfterCommand(Coordinate current, Coordinate start)
        {
            if (type == SVGCmdType.Absolute)
                return new Coordinate(x, y);
            else
                return current + new Coordinate(x, y);
        }
    }
}
