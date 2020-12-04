using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma.SVGCommands
{
    public enum SVGCmdType
    {
        Absolute,
        Relative
    }

    abstract class SVGCommand
    {
        public SVGCommand(SVGToken t)
        {
            if (char.IsLower(t.value[0]))
            {
                this.type = SVGCmdType.Absolute;
            }
            else
            {
                this.type = SVGCmdType.Relative;
            }
            this.command = t.value;
        }

        public string command { get; set; }
        public SVGCmdType type { get; set; }        

        public static SVGCommand GetCommandForToken(SVGToken t)
        {
            if (t.tokType != TokenType.Command)
            {
                throw new Exception("Invalid SVG File.  Unexpected token in path. '" + t.value + "'");
            }
            switch (t.value.ToUpper())
            {
                case "M":
                    return new SVGCommandMove(t);
                case "Z":
                    return new SVGCommandClosePath(t);
                case "L":
                    return new SVGCommandLineTo(t);
                case "H":
                    return new SVGCommandLineHorizontal(t);
                case "V":
                    return new SVGCommandLineVertical(t);
                case "C":
                    return new SVGCommandCubicCurve(t);
                case "S":
                    return new SVGCommandCubicSmooth(t);
                case "Q":
                    return new SVGCommandQuadCurve(t);
                case "T":
                    return new SVGCommandQuadSmooth(t);
                case "A":
                    return new SVGCommandEllipse(t);
                default:
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + t.value + "'");
            }
        }

        public abstract void ParseParameters(SVGTokenStream ts);
        public abstract Coordinate PositionAfterCommand(Coordinate current, Coordinate start);
    }
}
