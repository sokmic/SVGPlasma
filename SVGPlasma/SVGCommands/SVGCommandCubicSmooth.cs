using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma.SVGCommands
{
    class SVGCommandCubicSmooth : SVGCommand
    {
        public SVGCommandCubicSmooth(SVGToken t) : base(t) { }

        public decimal x2 { get; set; }
        public decimal y2 { get; set; }
        public decimal x { get; set; }
        public decimal y { get; set; }
        public override void ParseParameters(SVGTokenStream ts)
        {
            //(x2 y2 x y)
            //draw a curve from current point to (x,y)
            //The first control point is assumed to be the reflection of the second control point on the previous command relative to the current point
            //If there is no control point associated to the previous command, assume the first control point is coincident with the current point.
            //(x2,y2) is the second control point (end of the curve)            

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

            using (SVGToken y1t = ts.getToken())
            {
                if (y1t.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + y1t.value + "'");
                y = decimal.Parse(y1t.value);
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
* //"smooth" cubic bezier curve
                   //1st control point is the reflection of the prior command's 2nd control point, relative to the current point
                   //If there is no prior S or C command, 1st control point is the current point
                   lastQuadCP = null;
                   //prepare the parameters
                   cp = new SVGCoordPair[4];
                   cp[0] = new SVGCoordPair(lastx, lasty);
                   dp = (SVGDoubleCoordPair)cmd.pars[0];
                   if (lastCubicCP == null)
                       cp[1] = new SVGCoordPair(lastx, lasty);
                   else
                       cp[1] = new SVGCoordPair(lastx + (lastx - lastCubicCP.x), lasty + (lasty - lastCubicCP.y));                            
                   cp[2] = dp.p1;
                   cp[3] = dp.p2;
                   lastCubicCP = dp.p1;
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
