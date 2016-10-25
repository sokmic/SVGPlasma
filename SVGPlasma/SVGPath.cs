using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGPath
    {
        public System.Collections.Generic.List<SVGCommandGroup> cmdgroups = new List<SVGCommandGroup>();
        SVGTokenStream ts;

        //Parses the SVG file and prepares it for gcode generation
        public void Parse(string path)
        {
            
            byte[] txt = System.Text.Encoding.ASCII.GetBytes(path);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(txt);
            ts = new SVGTokenStream(ms);
            ParseCommandGroups();

            NormalizeCommands();

            CalcCurves();

            //create objects from the path (individual cuts to make)
            //CreateObjects();
            //sort the objects, inner most to outer most
            //SortObjects();            
            
        }

        //Breaks any commands with multiple argument sets into individual command objects
        //Also convert H and V commands to L commands for ease of handling later
        private void NormalizeCommands()
        {
            for(int i = 0; i< cmdgroups.Count;i++)
            {
                SVGCommandGroup sg = cmdgroups[i];
                SVGCoordPair lastmove = null;
                decimal lastx = 0;
                decimal lasty = 0;
                for(int j = 0; j < sg.commands.Count; j++)
                {
                    SVGCommand cmd = sg.commands[j];                    
                    string clonecmd = cmd.command;
                    if (clonecmd == "M")
                        clonecmd = "L";
                    while (cmd.pars.Count > 1)
                    {
                        SVGCommand clone = new SVGCommand();
                        clone.command = clonecmd;
                        clone.type = cmd.type;
                        clone.pars.Add(cmd.pars[cmd.pars.Count - 1]);
                        cmd.pars.RemoveAt(cmd.pars.Count - 1);
                        sg.commands.Insert(j + 1, clone);
                    }
                    switch (cmd.command)
                    {
                        case "M":
                            lastmove = (SVGCoordPair)cmd.pars[0];
                            lastx = lastmove.x;
                            lasty = lastmove.y;
                            break;
                        case "L":
                            lastx = ((SVGCoordPair)cmd.pars[0]).x;
                            lasty = ((SVGCoordPair)cmd.pars[0]).y;
                            break;
                        case "Z":
                            lastx = lastmove.x;
                            lasty = lastmove.y;
                            break;
                        case "C":
                            lastx = ((SVGTripleCoordPair)cmd.pars[0]).p3.x;
                            lasty = ((SVGTripleCoordPair)cmd.pars[0]).p3.y;
                            break;
                        case "S":
                            lastx = ((SVGDoubleCoordPair)cmd.pars[0]).p2.x;
                            lasty = ((SVGDoubleCoordPair)cmd.pars[0]).p2.y;
                            break;
                        case "Q":
                            lastx = ((SVGDoubleCoordPair)cmd.pars[0]).p2.x;
                            lasty = ((SVGDoubleCoordPair)cmd.pars[0]).p2.y;
                            break;
                        case "T":
                            lastx = ((SVGCoordPair)cmd.pars[0]).x;
                            lasty = ((SVGCoordPair)cmd.pars[0]).y;
                            break;
                        case "A":
                            lastx = ((SVGEArcArgs)cmd.pars[0]).p.x;
                            lasty = ((SVGEArcArgs)cmd.pars[0]).p.y;
                            break;
                        case "H":
                            cmd.command = "L";
                            if (cmd.type == SVGCmdType.Relative)
                                cmd.pars[0] = new SVGCoordPair(((SVGCoord)cmd.pars[0]).n, 0);
                            else
                                cmd.pars[0] = new SVGCoordPair(((SVGCoord)cmd.pars[0]).n, lasty);
                            break;
                        case "V":
                            cmd.command = "H";
                            if (cmd.type == SVGCmdType.Relative)
                                cmd.pars[0] = new SVGCoordPair(0,((SVGCoord)cmd.pars[0]).n);
                            else
                                cmd.pars[0] = new SVGCoordPair(lastx,((SVGCoord)cmd.pars[0]).n);
                            break;
                        default:
                            break;  
                    }                                       
                }                
            }
        }

        //Since the gcode command set available doesn't support curves, need to convert any curves to a series of line segments
        private void CalcCurves()
        {
            for (int i = 0; i < cmdgroups.Count; i++)
            {
                SVGCoordPair lastCubicCP = null;
                SVGCoordPair lastQuadCP = null;
                SVGCommandGroup sg = cmdgroups[i];
                SVGCoordPair lastmove = null;
                decimal lastx = 0;
                decimal lasty = 0;
                SVGCoordPair[] cp;
                SVGTripleCoordPair tp;
                SVGDoubleCoordPair dp;
                List<SVGCoordPair> points;
                for (int j = 0; j < sg.commands.Count; j++)
                {
                    SVGCommand cmd = sg.commands[j];
                    switch (cmd.command)
                    {
                        case "C":
                            //Cubic bezier curve
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
                            foreach(SVGCoordPair p in points)
                            {
                                SVGCommand newcmd = new SVGCommand();
                                newcmd.command = "L";
                                newcmd.type = SVGCmdType.Absolute;
                                newcmd.pars.Add(p);
                                sg.commands.Insert(j + 1, newcmd);
                            }
                            //remove the current command
                            sg.commands.RemoveAt(j);
                            break;
                        case "S":
                            //"smooth" cubic bezier curve
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
                            foreach (SVGCoordPair p in points)
                            {
                                SVGCommand newcmd = new SVGCommand();
                                newcmd.command = "L";
                                newcmd.type = SVGCmdType.Absolute;
                                newcmd.pars.Add(p);
                                sg.commands.Insert(j + 1, newcmd);
                            }
                            //remove the current command
                            sg.commands.RemoveAt(j);
                            break;
                        case "Q":
                            //Quadratic bezier curve
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
                            foreach (SVGCoordPair p in points)
                            {
                                SVGCommand newcmd = new SVGCommand();
                                newcmd.command = "L";
                                newcmd.type = SVGCmdType.Absolute;
                                newcmd.pars.Add(p);
                                sg.commands.Insert(j + 1, newcmd);
                            }
                            //remove the current command
                            sg.commands.RemoveAt(j);
                            break;
                        case "T":
                            //"smooth" quadratic bezier curve
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
                            foreach (SVGCoordPair p in points)
                            {
                                SVGCommand newcmd = new SVGCommand();
                                newcmd.command = "L";
                                newcmd.type = SVGCmdType.Absolute;
                                newcmd.pars.Add(p);
                                sg.commands.Insert(j + 1, newcmd);
                            }
                            //remove the current command
                            sg.commands.RemoveAt(j);
                            break;
                        case "A":
                            //elliptical arc
                            lastCubicCP = null;
                            lastQuadCP = null;
                            break;
                        case "M":
                            lastCubicCP = null;
                            lastQuadCP = null;
                            lastmove = (SVGCoordPair)cmd.pars[0];
                            lastx = lastmove.x;
                            lasty = lastmove.y;
                            break;
                        case "L":
                            lastCubicCP = null;
                            lastQuadCP = null;
                            lastx = ((SVGCoordPair)cmd.pars[0]).x;
                            lasty = ((SVGCoordPair)cmd.pars[0]).y;
                            break;
                        case "Z":
                            lastCubicCP = null;
                            lastQuadCP = null;
                            lastx = lastmove.x;
                            lasty = lastmove.y;
                            break;
                        default:
                            lastCubicCP = null;
                            lastQuadCP = null;
                            break;

                    }
                }
            }
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

        //Looks for any polygons (start==end or last command="Z") and separates them into their own groups.
        private void PolyCheck()
        {

        }

        private void ParseCommandGroups()
        {
            EatWS();
            while (ts.peek().tokType != TokenType.EOP)
            {                
                SVGCommandGroup cmdgrp = GetCommandGroup();
                cmdgroups.Add(cmdgrp);
                EatWS();
            }
        }

        private SVGCommandGroup GetCommandGroup()
        {
            SVGCommandGroup cg = new SVGCommandGroup();
            while(ts.peek().tokType != TokenType.EOP)
            {
                SVGCommand cmd = GetCommand();
                cg.commands.Add(cmd);
                EatWS();
                if (ts.peek().tokType == TokenType.Command && ts.peek().value.ToUpper() == "M")
                    break;
            }
            return cg;
        }

        private SVGCommand GetCommand()
        {
            EatWS();
            SVGToken t = ts.getToken();
                        
            if (t.tokType != TokenType.Command)
                InvalidToken(t);

            SVGCommand cmd = new SVGCommand();
            cmd.command = t.value;
            if (char.IsLower(cmd.command, 1))
                cmd.type = SVGCmdType.Relative;
            else
                cmd.type = SVGCmdType.Absolute;
            cmd.command = cmd.command.ToUpper();
            SVGCoordPair p;
            SVGCoordPair p1;
            SVGCoordPair p2;
            SVGCoordPair p3;
            SVGCoord c;

            switch (cmd.command)
            {
                case "M":
                    //move
                    EatWS();
                    p = getCoordPair();
                    cmd.pars.Add(p);
                    EatWSComma();
                    while(ts.peek().tokType == TokenType.Number)
                    {
                        p = getCoordPair();
                        cmd.pars.Add(p);
                        EatWSComma();
                    }
                    break;
                case "Z":
                    break;
                case "L":
                    EatWS();
                    p = getCoordPair();
                    cmd.pars.Add(p);
                    EatWSComma();
                    while(ts.peek().tokType == TokenType.Number)
                    {
                        p = getCoordPair();
                        cmd.pars.Add(p);
                        EatWSComma();
                    }
                    break;
                case "H":
                    EatWS();
                    c = getCoord();
                    cmd.pars.Add(c);
                    EatWSComma();
                    while(ts.peek().tokType == TokenType.Number)
                    {
                        c = getCoord();
                        cmd.pars.Add(c);
                        EatWSComma();
                    }
                    break;
                case "V":
                    EatWS();
                    c = getCoord();
                    cmd.pars.Add(c);
                    EatWSComma();
                    while(ts.peek().tokType == TokenType.Number)
                    {
                        c = getCoord();
                        cmd.pars.Add(c);
                        EatWSComma();
                    }
                    break;
                case "C":
                    //3 pairs
                    EatWS();
                    p1 = getCoordPair();
                    EatWSComma();
                    p2 = getCoordPair();
                    EatWSComma();
                    p3 = getCoordPair();
                    cmd.pars.Add(new SVGTripleCoordPair(p1, p2, p3));
                    EatWSComma();
                    while(ts.peek().tokType == TokenType.Number)
                    {
                        p1 = getCoordPair();
                        EatWSComma();
                        p2 = getCoordPair();
                        EatWSComma();
                        p3 = getCoordPair();
                        cmd.pars.Add(new SVGTripleCoordPair(p1, p2, p3));
                        EatWSComma();
                    }
                    break;
                case "S":
                    //2 pairs
                    EatWS();
                    p1 = getCoordPair();
                    EatWSComma();
                    p2 = getCoordPair();
                    cmd.pars.Add(new SVGDoubleCoordPair(p1,p2));
                    EatWSComma();                    
                    while (ts.peek().tokType == TokenType.Number)
                    {
                        p1 = getCoordPair();                        
                        EatWSComma();
                        p2 = getCoordPair();
                        cmd.pars.Add(new SVGDoubleCoordPair(p1, p2));
                        EatWSComma();                        
                    }
                    break;
                case "Q":
                    //2 pairs
                    EatWS();
                    p1 = getCoordPair();                    
                    EatWSComma();
                    p2 = getCoordPair();
                    cmd.pars.Add(new SVGDoubleCoordPair(p1, p2));
                    EatWSComma();
                    while (ts.peek().tokType == TokenType.Number)
                    {
                        p1 = getCoordPair();
                        EatWSComma();
                        p2 = getCoordPair();
                        cmd.pars.Add(new SVGDoubleCoordPair(p1, p2));
                        EatWSComma();
                    }
                    break;
                case "T":
                    EatWS();
                    p = getCoordPair();
                    cmd.pars.Add(p);
                    EatWSComma();
                    while(ts.peek().tokType == TokenType.Number)
                    {
                        p = getCoordPair();
                        cmd.pars.Add(p);
                        EatWSComma();
                    }
                    break;
                case "A":
                    //nonnegative-number comma-wsp? nonnegative-number comma-wsp? number comma-wsp flag comma-wsp? flag comma-wsp? coordinate-pair
                    EatWS();
                    decimal n1 = getNonNegNumber();
                    EatWSComma();
                    decimal n2 = getNonNegNumber();
                    EatWSComma();
                    decimal n3 = getNumber();
                    if (ts.peek().tokType != TokenType.Whitespace && ts.peek().tokType != TokenType.Comma)
                        InvalidToken(ts.getToken());
                    EatWSComma();
                    bool flag1 = getFlag();
                    EatWSComma();
                    bool flag2 = getFlag();
                    EatWSComma();
                    p = getCoordPair();
                    cmd.pars.Add(new SVGEArcArgs(n1, n2, n3, flag1, flag2, p));
                    EatWSComma();
                    while (ts.peek().tokType == TokenType.Number)
                    {
                        n1 = getNonNegNumber();
                        EatWSComma();
                        n2 = getNonNegNumber();
                        EatWSComma();
                        n3 = getNumber();
                        if (ts.peek().tokType != TokenType.Whitespace && ts.peek().tokType != TokenType.Comma)
                            InvalidToken(ts.getToken());
                        EatWSComma();
                        flag1 = getFlag();
                        EatWSComma();
                        flag2 = getFlag();
                        EatWSComma();
                        p = getCoordPair();
                        cmd.pars.Add(new SVGEArcArgs(n1, n2, n3, flag1, flag2, p));
                        EatWSComma();
                    }
                    break;
                default:
                    InvalidToken(t);
                    break;
            }
            return cmd;
        }
        
        private void EatWS()
        {
            while (ts.peek().tokType == TokenType.Whitespace)
                ts.getToken();
        }

        private void EatWSComma()
        {
            EatWS();
            if (ts.peek().tokType == TokenType.Comma)
                ts.getToken();
            EatWS();
        }

        private SVGCoordPair getCoordPair()
        {            
            SVGToken n1 = ts.getToken();
            if (n1.tokType != TokenType.Number)
                InvalidToken(n1);
            EatWSComma();
            SVGToken n2 = ts.getToken();
            if (n2.tokType != TokenType.Number)
                InvalidToken(n2);
            return new SVGCoordPair(decimal.Parse(n1.value), decimal.Parse(n2.value));
        }

        private SVGCoord getCoord()
        {
            SVGToken n1 = ts.getToken();
            if (n1.tokType != TokenType.Number)
                InvalidToken(n1);
            return new SVGCoord(decimal.Parse(n1.value));
        }

        private decimal getNonNegNumber()
        {
            SVGToken n1 = ts.getToken();
            if (n1.tokType != TokenType.Number)
                InvalidToken(n1);
            decimal d = decimal.Parse(n1.value);
            if (d < 0)
                InvalidToken(n1);
            return d;
        }

        private decimal getNumber()
        {
            SVGToken n1 = ts.getToken();
            if (n1.tokType != TokenType.Number)
                InvalidToken(n1);
            decimal d = decimal.Parse(n1.value);
            return d;
        }

        private bool getFlag()
        {
            SVGToken n1 = ts.getToken();
            if (n1.tokType != TokenType.Number)
                InvalidToken(n1);
            decimal d = decimal.Parse(n1.value);
            if (d == 0)
                return false;
            else if (d == 1)
                return true;
            else
            {
                InvalidToken(n1);
                return false;  //to shut up the compiler
            }
        }


        private void InvalidToken(SVGToken t)
        {
            throw new Exception("Invalid SVG File.  Unexpected token in path. '" + t.value + "'");
        }

        /*        
        private void SortObjects()
        {
            bool changed = false;
            do
            {
                changed = false;
                //start with the second object in the list
                for (int i = 1; i < objects.Count; i++)
                {
                    //if the first point of the current object is contained in the previous object
                    //we can assume the current object is inside the previous object and should be cut first
                    if (objects[i - 1].isPointInside(objects[i].points[0]))
                    {
                        //switch the current and previous object, making the current object to be cut first
                        changed = true;
                        SVGObject t = objects[i - 1];
                        objects[i - 1] = objects[i];
                        objects[i] = t;
                    }
                }
            } while (changed);
        }        
        */
    }
}
