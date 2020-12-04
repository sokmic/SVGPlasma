using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVGPlasma.SVGCommands
{
    class SVGCommandEllipse : SVGCommand
    {
        public SVGCommandEllipse(SVGToken t) : base(t) { }

        public decimal rx { get; set; }
        public decimal ry { get; set; }
        public decimal xRotation { get; set; }
        public bool largeArcFlag { get; set; }
        public bool sweepFlag { get; set; }
        public decimal x { get; set; }
        public decimal y { get; set; }
        public override void ParseParameters(SVGTokenStream ts)
        {
            //(rx ry x-axis-rotation large-arc-flag sweep-flag x y)
            //draw an elliptical arc from current point to (x, y)
            //rx and ry are radii in the x and y axis
            //the x-axis of the ellipse is rotated by x-axis-rotation relative to the x-axis of the current coordinate system
            //given this information 4 arcs could be drawn
            //large-arc-flag determines if the arc sweep >= 180 degrees or <= 180 degrees will be chosen (large vs small)
            //sweep-flag determines the direction of the sweep, 1 = positive, 0 = negative

            using (SVGToken rxt = ts.getToken())
            {
                if (rxt.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + rxt.value + "'");
                rx = decimal.Parse(rxt.value);
            }
            using (SVGToken ryt = ts.getToken())
            {
                if (ryt.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + ryt.value + "'");
                ry = decimal.Parse(ryt.value);
            }
            using (SVGToken rott = ts.getToken())
            {
                if (rott.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + rott.value + "'");
                xRotation = decimal.Parse(rott.value);
            }
            using (SVGToken laFlagt = ts.getToken())
            {
                if (laFlagt.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + laFlagt.value + "'");
                largeArcFlag = (laFlagt.value == "1");
            }
            using (SVGToken sFlagt = ts.getToken())
            {
                if (sFlagt.tokType != TokenType.Number)
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + sFlagt.value + "'");
                sweepFlag = (sFlagt.value == "1");
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
* //elliptical arc
                   lastCubicCP = null;
                   lastQuadCP = null;
                   //convert endpoint parameters to center based
                   SVGEArcArgs args = (SVGEArcArgs)cmd.pars[0];
                   double x1 = (double)lastx;
                   double y1 = (double)lasty;
                   double x2 = (double)args.p.x;
                   double y2 = (double)args.p.y;
                   bool fA = args.flag1;
                   bool fS = args.flag2;
                   double rx = (double)args.n1;
                   double ry = (double)args.n2;
                   double phi = (double)args.n3 * (Math.PI / 180);

                   double x1prime = (Math.Cos(phi) * ((x1 - x2) / 2) + Math.Sin(phi) * ((x1 - x2) / 2));
                   double y1prime = ((Math.Sin(phi) * -1) * ((y1 - y2) / 2) + Math.Cos(phi) * ((y1 - y2) / 2));
                   double t1 = Math.Sqrt((Math.Pow(rx,2) * Math.Pow(ry,2) - Math.Pow(rx,2) * Math.Pow(y1prime,2) - Math.Pow(ry,2) * Math.Pow(x1prime,2)) / (Math.Pow(rx,2) * Math.Pow(y1prime,2) + Math.Pow(ry,2) * Math.Pow(x1prime,2)));
                   if (fA && fS) t1 *= -1;
                   double cxprime = t1 * ((rx * y1prime) / ry);
                   double cyprime = t1 * (-1 * ((ry * x1prime) / rx));
                   double cx = (Math.Cos(phi) * cxprime + (-1 * Math.Sin(phi)) * cxprime) + ((x1 + x2) / 2);
                   double cy = (Math.Sin(phi) * cyprime + Math.Cos(phi) * cyprime) + ((y1 + y2) / 2);
                   double ang1 = angleFunc(new SVGCoordPair(1M, 0M), new SVGCoordPair((x1prime - cxprime) / rx, (y1prime - cyprime) / ry));
                   SVGCoordPair u = new SVGCoordPair((x1prime - cxprime) / rx, (y1prime - cyprime) / ry);
                   SVGCoordPair v = new SVGCoordPair((-1 * x1prime - cxprime) / rx, (-1 * y1prime - cyprime) / ry);
                   double angdelta = angleFunc(u, v) % 360;
                   if (!fS && angdelta > 0)
                       angdelta = angdelta - 360;
                   else if (fS && angdelta < 0)
                       angdelta = angdelta + 360;
                   //now the equation is
                   //x = xcenter + rx * cos(t)
                   //y = ycenter + ry * sin(t)
                   //with t going from ang1 to ang1 + angdelta
                   decimal tdelt = (decimal)angdelta / 25; //do 25 subdivisions
                   decimal tfin = (decimal)angdelta;
                   //generate the points
                   //for (decimal t = (decimal)ang1; t <= tfin; t += tdelt)
                   for (decimal t = tfin; t >= (decimal)ang1; t-= tdelt)                            
                   {
                       SVGCommand newcmd = new SVGCommand();
                       newcmd.command = "L";
                       newcmd.type = SVGCmdType.Absolute;
                       double a = ((double)t) * (Math.PI / 180);
                       decimal ptx = (decimal)cx + args.n1 * (decimal)Math.Cos(a);
                       decimal pty = (decimal)cy + args.n2 * (decimal)Math.Sin(a);
                       newcmd.pars.Add(new SVGCoordPair(ptx, pty));
                       sg.commands.Insert(j + 1, newcmd);
                   }
                   //remove the current command
                   sg.commands.RemoveAt(j);*/
    }
}
