using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma.SVGCommands
{
    class SVGCommandQuadCurve : SVGCommand
    {
        public SVGCommandQuadCurve(SVGToken t) : base(t) { }

        public decimal x1 { get; set; }
        public decimal y1 { get; set; }
        public decimal x { get; set; }
        public decimal y { get; set; }
        public override void ParseParameters(SVGTokenStream ts)
        {
            //(x1 y1 x y)
            //draw a curve from current point to (x,y) using (x1,y1) as the control point

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
* //Quadratic bezier curve
                   lastCubicCP = null;

                   //prepare the parameters
                   cp = new SVGCoordPair[3];
                   cp[0] = new SVGCoordPair(lastx, lasty);
                   dp = (SVGDoubleCoordPair)cmd.pars[0];
                   cp[1] = dp.p1;
                   cp[2] = dp.p2;
                   lastQuadCP = dp.p1;
                   //get the list of points
                   points = getBezierPoints(cp);
                   //add them to the list of commands after the current command
                   for(int w = points.Count-1; w >= 0; w--)
                   {
                       SVGCoordPair p = points[w];
                       SVGCommand newcmd = new SVGCommand();
                       newcmd.command = "L";
                       newcmd.type = SVGCmdType.Absolute;
                       newcmd.pars.Add(p);
                       sg.commands.Insert(j + 1, newcmd);
                   }
                   //remove the current command
                   sg.commands.RemoveAt(j);*/
    }
}
