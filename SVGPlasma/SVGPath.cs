using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using SVGPlasma.SVGCommands;

namespace SVGPlasma
{
    class SVGPath
    {        
        List<SVGCommand> commands { get; set; }

        //Parses the SVG file and prepares it for gcode generation
        public void Parse(string path)
        {            
            byte[] txt = System.Text.Encoding.ASCII.GetBytes(path);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(txt);
            SVGTokenStream ts = new SVGTokenStream(ms);

            List<SVGCommand> commands = ParseAllCommands(ts);
            ms.Close();
            ms.Dispose();
        }

        private List<SVGCommand> ParseAllCommands(SVGTokenStream ts)
        {
            List<SVGCommand> commands = new List<SVGCommand>();
            while (ts.peek().tokType != TokenType.EOP)
            {
                //get the token
                var cmdToken = ts.getToken();
                if(cmdToken.tokType != TokenType.Command)
                {
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + cmdToken.value + "'");
                }
                if(commands.Count == 0 && cmdToken.value.ToUpper() != "M")
                {
                    throw new Exception("Invalid SVG File.  Unexpected token in path. '" + cmdToken.value + "'.  Path must begin with an M command");
                }

                SVGCommand cmd;
                while (ts.peek().tokType == TokenType.Number)
                {
                    //if the first command in a path is a relative move, treat it as an absolute move
                    //all subsequent moves are then relative
                    if (cmdToken.value == "m" && commands.Count == 0)
                    {
                        SVGToken tmp = new SVGToken("M", cmdToken.tokType);
                        cmd = SVGCommand.GetCommandForToken(tmp);
                    }
                    else
                    {
                        cmd = SVGCommand.GetCommandForToken(cmdToken);
                    }
                    //parse parameters for the token
                    cmd.ParseParameters(ts);
                    commands.Add(cmd);

                    //If a moveto is followed by multiple pairs of coordinates, the subsequent pairs are treated as implicit lineto commands
                    if (cmdToken.value == "m") cmdToken.value = "l";
                    if (cmdToken.value == "M") cmdToken.value = "L";
                    //each set of numbers after a command is taken as another instance of that command until the next command is reached
                }                
            }
            return commands;
        }               
        
        public List<SVGPath> Split()
        {
            //Split the path into subpaths
            //Everytime the position goes back to the start position, split off a new subpath

            var paths = new List<SVGPath>();
            var curPath = new SVGPath();
            //first command should be a move, add it to the current subpath
            curPath.commands.Add(commands[0]);
            var firstMove = (SVGCommandMove)commands[0];
            var startPos = new Coordinate(firstMove.x, firstMove.y);
            var curPos = new Coordinate(firstMove.x,firstMove.y);
            //go through all commands but the first, which we already handled
            for (int i = 1; i < commands.Count; i++)
            {
                //add the command to the subpath and set to new position
                var cmd = commands[i];
                curPath.commands.Add(cmd);
                curPos = cmd.PositionAfterCommand(curPos, startPos);
                //if we've come back to the position of the initial move
                if(curPos == startPos)
                {
                    //add the subpath to the list, start a new one and add a move to the initial point to start it
                    paths.Add(curPath);
                    curPath = new SVGPath();
                    curPath.commands.Add(firstMove);
                }
            }
            //if we've hit the end and we didn't come back to the start
            if(curPos != startPos)
            {
                //add the remaining subpath
                paths.Add(curPath);
            }
            return paths;
        }


        /*
        private double angleFunc(SVGCoordPair u, SVGCoordPair v)
        {
            int sign = Math.Sign(u.x * v.y - u.y * v.x);
            double n = (double)(u.x * v.x + u.y * v.y);
            double d = Math.Sqrt((double)(u.x * u.x + u.y * u.y)) * Math.Sqrt((double)(v.x * v.x + v.y * v.y));
            return sign * (Math.Acos(n / d) / (Math.PI / 180));
        }

        private List<SVGCoordPair> getBezierPoints(SVGCoordPair[] points)
        {
            List<SVGCoordPair> bpoints = new List<SVGCoordPair>();
            for (decimal t = 0.0M; t <=1.0M; t+=0.04M)
            {
                bpoints.Add(getSingleBezierPoint(points,t));
            }
            return bpoints;
        }

        private SVGCoordPair getSingleBezierPoint(SVGCoordPair[] points, decimal t)
        {
            if(points.Length==1)
                return(points[0]);
            else
            {
                SVGCoordPair[] np = new SVGCoordPair[points.Length-1];                
                for (int i=0; i<np.Length; i++)
                {
                    decimal x = (1.0M - t) * points[i].x + t * points[i + 1].x;
                    decimal y = (1.0M - t) * points[i].y + t * points[i + 1].y;
                    np[i] = new SVGCoordPair(x,y);
                }      
                return getSingleBezierPoint(np,t);
            }    
        } 
        */
    }
}
