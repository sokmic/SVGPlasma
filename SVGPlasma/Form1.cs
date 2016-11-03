using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SVGPlasma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.FileOk += openFileDialog1_FileOk;
        }

        void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            txtFileName.Text = openFileDialog1.FileName;
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();            
        }

        private void cmdGenerate_Click(object sender, EventArgs e)
        {
            GCodeMachineSettings gcmach = new GCodeMachineSettings();
            GCodeMaterialSettings gcmat = new GCodeMaterialSettings();
            string gcfilename = System.IO.Path.ChangeExtension(txtFileName.Text, "gcode");

            string xmlstr = System.IO.File.ReadAllText(txtFileName.Text);
            System.IO.StreamWriter sout = System.IO.File.CreateText(gcfilename);

            sout.WriteLine("; Generated " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));

            //import the SVG file
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlstr);
            XmlNamespaceManager ns = new XmlNamespaceManager(xml.NameTable);
            ns.AddNamespace("svg", "http://www.w3.org/2000/svg");
            XmlNode svg = xml.DocumentElement;            
            XmlNode title = svg.SelectSingleNode("svg:title",ns);
            XmlNode desc = svg.SelectSingleNode("svg:desc",ns);
            XmlNode path = svg.SelectSingleNode("svg:path",ns);
            if (title != null)
            {
                sout.WriteLine("; Object Title: " + title.InnerText);             
            }
            if (desc != null)
            {
                sout.WriteLine("; Object Description: " + desc.InnerText);
            }

            SVGPath p = new SVGPath();
            if (path != null)
            {
                XmlNode d = path.Attributes.GetNamedItem("d");
                string svgobj = d.Value;
                p.Parse(svgobj);
            }
            
            //adjust for the width of the cutter
            if (gcmach.CutWidth > 0)
            {
                for(int j = 0; j < p.objects.Count; j++)
                {
                    SVGObject o = p.objects[j];
                    bool lastobj = (j == p.objects.Count - 1);
                    //if the object is a polygon if first and last points are the same and there are at least 3 vertices.
                    if(o.points[0].x == o.points[o.points.Count-1].x && o.points[0].y == o.points[o.points.Count-1].y && o.points.Count > 3)
                    {
                        //store the original values for use in calculations
                        foreach(SVGCoordPair pt in o.points)
                        {
                            pt.xOrig = pt.x;
                            pt.yOrig = pt.y;
                        }
                        //last point and first point are the same.  Only have to do one of the two.
                        for (int i = 0; i < o.points.Count - 1; i++)
                        {
                            SVGCoordPair p1;
                            if (i == 0)
                                p1 = o.points[o.points.Count - 2];
                            else
                                p1 = o.points[i - 1];
                            SVGCoordPair p2 = o.points[i];
                            SVGCoordPair p3;
                            if (i == o.points.Count - 2)
                                p3 = o.points[0];
                            else
                                p3 = o.points[i + 1];

                            //calculate the normal vector of segment 1 and segment 2
                            SVGCoordPair n1 = GetNormal(p1, p2);
                            SVGCoordPair n2 = GetNormal(p2, p3);

                            //the normal of the vertex will be the average of the 2 segment normals
                            SVGCoordPair nv = new SVGCoordPair((n1.x + n2.x) / 2, (n1.y + n2.y) / 2);

                            if(nv.x == 0)
                            {
                                //vertical line
                                //set the length on y only
                                nv.y = gcmach.CutWidth;
                            }
                            else if(nv.y == 0)
                            {
                                //horizontal line
                                //set the length on x only
                                nv.x = gcmach.CutWidth;
                            }
                            else
                            {
                                //get the slope
                                decimal a = nv.y / nv.x;
                                nv = GetVector(a, gcmach.CutWidth);
                            }

                            SVGCoordPair t = new SVGCoordPair(p2.xOrig + nv.x, p2.yOrig + nv.y);
                            //if the point is inside the poly and it isn't the outer most object, we're good
                            if (o.isPointInside(t) && !lastobj)
                            {
                                p2.x = t.x;
                                p2.y = t.y;
                            }
                            else
                            {
                                // need to invert the normal vector
                                p2.x = p2.xOrig - nv.x;
                                p2.y = p2.yOrig - nv.y;
                            }
                        }
                        //copy the first point to the last point
                        o.points[o.points.Count - 1].x = o.points[0].x;
                        o.points[o.points.Count - 1].y = o.points[0].y;
                    }
                }
            }

            //Generate the G-Code
            sout.Write(gcmach.BeginCode);
            sout.WriteLine();
            sout.WriteLine("G21 ; All units in mm");

            foreach(SVGObject o in p.objects)
            {
                sout.WriteLine("G0 " + "X" + o.points[0].x.ToString() + " Y" + o.points[0].y.ToString());
                sout.WriteLine(gcmach.SpindleOnCode);
                sout.WriteLine("G4 P" + gcmat.PierceTime.ToString() + " ; penetrate");
                foreach (SVGCoordPair pt in o.points)
                {
                    sout.WriteLine("G1 " + "X" + pt.x.ToString() + " Y" + pt.y.ToString() + " F" + gcmat.FeedRate.ToString());
                }
                sout.WriteLine(gcmach.SpindleOffCode);
            }
            sout.Write(gcmach.EndCode);
            sout.WriteLine();            
            sout.Close();             
            MessageBox.Show("File generated");
        }

        private SVGCoordPair GetNormal(SVGCoordPair p1, SVGCoordPair p2)
        {
            //Get the normal vector of the line segment from p1 to p2
            if (p1.xOrig == p2.xOrig)
            {
                //vertical line
                //normal is horizontal line
                return new SVGCoordPair(1M, 0M);
            }
            if (p1.yOrig == p2.yOrig)
            {
                //horizontal line
                //normal is a vertical line
                return new SVGCoordPair(0M, 1M);
            }
            //calculate the slope of the line
            decimal a = (p2.yOrig - p1.yOrig) / (p2.xOrig - p1.xOrig);
            //slope of the normal - reciprocal of the negative
            decimal an = -1 / a;
            //get a vector of length 1 with slope an
            return GetVector(an, 1);            
        }

        private SVGCoordPair GetVector(decimal a, decimal l)
        {
            //generate a vector with slope of a and length of l
            // y=ax  and l^2 = x^2 + y^2
            // l^2 = x^2 + (ax)^2
            // l^2 = (a^2 + 1)x^2
            // l^2/(a^2 + 1) = x^2

            decimal x = (decimal)Math.Sqrt(Math.Pow((double)l,2) / (Math.Pow((double)a, 2) + 1));
            decimal y = a * x;

            return new SVGCoordPair(x, y);
        }
    }
}
