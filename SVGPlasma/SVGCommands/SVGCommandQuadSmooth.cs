using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma.SVGCommands
{
    class SVGCommandQuadSmooth : SVGCommand
    {
        public SVGCommandQuadSmooth(SVGToken t) : base(t) { }
        
        public decimal x { get; set; }
        public decimal y { get; set; }
        public override void ParseParameters(SVGTokenStream ts)
        {
            //(x y)
            //draw a curve from current point to (x,y)
            //The control point is assumed to be the reflection of the control point on the previous command relative to the current point
            //If there is no control point associated to the previous command, assume the control point is coincident with the current point.                        

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
* //"smooth" quadratic bezier curve
                   //control point is the reflection of the prior command's control point, relative to the current point
                   //If there is no prior Q or T command, control point is the current point
                   lastCubicCP = null;
                   //prepare the parameters
                   cp = new SVGCoordPair[3];
                   cp[0] = new SVGCoordPair(lastx, lasty);
                   SVGCoordPair pt = (SVGCoordPair)cmd.pars[0];
                   if (lastQuadCP == null)
                       cp[1] = new SVGCoordPair(lastx, lasty);
                   else
                       cp[1] = new SVGCoordPair(lastx + (lastx - lastQuadCP.x), lasty + (lasty - lastQuadCP.y));
                   cp[2] = pt;
                   lastQuadCP = cp[1];
                   //get the list of points
                   points = getBezierPoints(cp);
                   //add them to the list of commands after the current command
                   for (int w = points.Count - 1; w >= 0; w--)
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
