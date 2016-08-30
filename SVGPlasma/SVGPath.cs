using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGPath
    {
        public System.Collections.Generic.List<SVGCommand> commands = new List<SVGCommand>();
        public System.Collections.Generic.List<SVGObject> objects = new List<SVGObject>();

        public void Parse(string path)
        {
            byte[] txt = System.Text.Encoding.ASCII.GetBytes(path);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(txt);
            SVGTokenStream ts = new SVGTokenStream(ms);








            //read in the path
            ParseCommand(path);
            //create objects from the path (individual cuts to make)
            CreateObjects();
            //sort the objects, inner most to outer most
            SortObjects();            
        }

        private void ParseCommand(string path)
        {
            if (path == "")
                return;

            path = path.Trim();
            SVGCommand c = new SVGCommand();
            int i = 0;
            string t = "";
            decimal x = 0;
            decimal y = 0;
            switch (path.Substring(0, 1))
            {
                case "M":
                case "m":
                    c.command = path.Substring(0, 1);
                    c.type = SVGCmdType.Absolute;
                    if (c.command == "m") c.type = SVGCmdType.Relative;
                    c.command = c.command.ToUpper();
                    path = path.Substring(1).Trim();
                    //get the x coordinate
                    i = path.IndexOf(" ");
                    t = path.Substring(0, i);
                    x = decimal.Parse(t);
                    path = path.Substring(i).Trim();
                    //get the y coordinate
                    i = path.IndexOf(" ");
                    t = path.Substring(0, i);
                    y = decimal.Parse(t);
                    path = path.Substring(i).Trim();
                    c.point = new SVGPoint(x, y);
                    commands.Add(c);
                    break;
                case "L":
                case "l":
                    c.command = path.Substring(0, 1);
                    c.type = SVGCmdType.Absolute;
                    if (c.command == "l") c.type = SVGCmdType.Relative;
                    c.command = c.command.ToUpper();
                    path = path.Substring(1).Trim();
                    //get the x coordinate
                    i= path.IndexOf(" ");
                    t = path.Substring(0, i);
                    x = decimal.Parse(t);
                    path = path.Substring(i).Trim();
                    //get the y coordinate
                    i = path.IndexOf(" ");
                    t = path.Substring(0, i);
                    y = decimal.Parse(t);
                    path = path.Substring(i).Trim();
                    c.point = new SVGPoint(x, y);
                    commands.Add(c);
                    break;                    
                case "Z":
                case "z":
                    c.command = path.Substring(0, 1);
                    c.type = SVGCmdType.Absolute;
                    if (c.command == "z") c.type = SVGCmdType.Relative;
                    c.command = c.command.ToUpper();
                    path = path.Substring(1).Trim();
                    commands.Add(c);
                    break;
                default:
                    throw new Exception("Unknown path command '" + path.Substring(0,1) + "'");
            }
            ParseCommand(path);
        }

        private void CreateObjects()
        {
            SVGObject o = new SVGObject();
            SVGPoint last = null;
            SVGPoint p = null;
            SVGPoint first = null;
            foreach (SVGCommand cmd in commands)
            {
                switch (cmd.command)
                {
                    case "M":
                        p = cmd.point;
                        if (cmd.type == SVGCmdType.Relative)
                        {
                            p.Add(last);
                        }
                        first = p;
                        o.points.Add(p);
                        last = p;
                        break;
                    case "L":
                        p = cmd.point;
                        if (cmd.type == SVGCmdType.Relative)
                        {
                            p.Add(last);
                        }
                        o.points.Add(p);
                        last = p;
                        break;
                    case "Z":
                        o.points.Add(first);
                        objects.Add(o);
                        o = new SVGObject();
                        break;
                }
            }
        }
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
    }
}
