using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma.SVGCommands
{
    class SVGCommandCubicCurve : SVGCommand
    {
        public SVGCommandCubicCurve(SVGToken t) : base(t) { }

        public decimal x1 { get; set; }
        public decimal y1 { get; set; }
        public decimal x2 { get; set; }
        public decimal y2 { get; set; }
        public decimal x { get; set; }
        public decimal y { get; set; }
        public override void ParseParameters(SVGTokenStream ts)
        {
            //(x1 y1 x2 y2 x y)
            //draw a curve from current point to (x,y) using (x1,y1) and (x2,y2) as control points at the start and end of the curve

            using (SVGToken x1t = ts.getToken())
            {
                if (x1t.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + x1t.value + "'");
                x1 = decimal.Parse(x1t.value);
            }

            using (SVGToken y1t = ts.getToken())
            {
                if (y1t.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + y1t.value + "'");
                y1 = decimal.Parse(y1t.value);
            }

            using (SVGToken x2t = ts.getToken())
            {
                if (x2t.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + x2t.value + "'");
                x2 = decimal.Parse(x2t.value);
            }

            using (SVGToken y2t = ts.getToken())
            {
                if (y2t.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + y2t.value + "'");
                y2 = decimal.Parse(y2t.value);
            }

            using (SVGToken xt = ts.getToken())
            {
                if (xt.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + xt.value + "'");
                x = decimal.Parse(xt.value);
            }

            using (SVGToken yt = ts.getToken())
            {
                if (yt.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + yt.value + "'");
                y = decimal.Parse(yt.value);
            }                            
        }

        public override Coordinate PositionAfterCommand(Coordinate current, Coordinate start)
        {
            if (type == SVGCmdType.Absolute)
                return new Coordinate(x, y);
            else
                return current + new Coordinate(x, y);
        }
        /*
* //Cubic bezier curve
                   lastQuadCP = null;
                   //prepare the parameters
                   cp = new SVGCoordPair[4];
                   cp[0] = new SVGCoordPair(lastx,lasty);
                   tp = (SVGTripleCoordPair)cmd.pars[0];
                   cp[1] = tp.p1;
                   cp[2] = tp.p2;
                   cp[3] = tp.p3;
                   lastCubicCP = tp.p2;
                   //get the list of points
                   points = getBezierPoints(cp);
                   //add them to the list of commands after the current command
                   for(int w = points.Count-1; w >=0; w--)
                   {
                       SVGCoordPair p = points[w];
                       SVGCommand newcmd = new SVGCommand();
                       newcmd.command = "L";
                       newcmd.type = SVGCmdType.Absolute;
                       newcmd.pars.Add(p);
                       sg.commands.Insert(j + 1, newcmd);
                   }
                   //remove the current command
                   sg.commands.RemoveAt(j);
*/
    }
}
