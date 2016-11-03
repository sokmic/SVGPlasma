using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGPlasma
{
    class SVGObject
    {
        public System.Collections.Generic.List<SVGCoordPair> points = new List<SVGCoordPair>();

        public SVGCoordPair this[int index]
        {
            get
            {
                return points[index];
            }
            set
            {
                points[index] = value;
            }
        }

        public bool isPointInside(SVGCoordPair p)
        {
            //the SVGObject passed in will have the "origin" vertex specified twice.  Remove one copy for this.
            SVGObject polygon = new SVGObject();
            for (int j = 1; j < this.points.Count; j++)
            {
                polygon.points.Add(this.points[j]);
            }

            // There must be at least 3 vertices in polygon
            if (polygon.points.Count - 1 < 3) return false;
            int n = polygon.points.Count - 1;

            // Create a point for line segment from p to "infinite" x
            SVGCoordPair extreme = new SVGCoordPair(100000, p.y);

            // Count intersections of the above line with sides of polygon
            int count = 0, i = 0;
            do
            {
                int next = (i + 1) % n;

                // Check if the line segment from 'p' to 'extreme' intersects
                // with the line segment from 'polygon[i]' to 'polygon[next]'
                if (doIntersect(polygon[i], polygon[next], p, extreme))
                {
                    // If the point 'p' is colinear with line segment 'i-next',
                    // then check if it lies on segment. If it lies, return true,
                    // otherwise false
                    if (orientation(polygon[i], p, polygon[next]) == 0)
                        return onSegment(polygon[i], p, polygon[next]);

                    count++;
                }
                i = next;
            } while (i != 0);

            // Return true if count is odd, false otherwise
            return count % 2 == 1;
        }

        // Given three colinear points p, q, r, the function checks if
        // point q lies on line segment 'pr'
        private bool onSegment(SVGCoordPair p, SVGCoordPair q, SVGCoordPair r)
        {
            if (q.x <= Math.Max(p.x, r.x) && q.x >= Math.Min(p.x, r.x) &&
                    q.y <= Math.Max(p.y, r.y) && q.y >= Math.Min(p.y, r.y))
                return true;
            return false;
        }

        // To find orientation of ordered triplet (p, q, r).
        // The function returns following values
        // 0 --> p, q and r are colinear
        // 1 --> Clockwise
        // 2 --> Counterclockwise
        int orientation(SVGCoordPair p, SVGCoordPair q, SVGCoordPair r)
        {
            decimal val = (q.y - p.y) * (r.x - q.x) -
                      (q.x - p.x) * (r.y - q.y);

            if (val == 0) return 0;  // colinear
            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        // The function that returns true if line segment 'p1q1'
        // and 'p2q2' intersect.
        bool doIntersect(SVGCoordPair p1, SVGCoordPair q1, SVGCoordPair p2, SVGCoordPair q2)
        {
            // Find the four orientations needed for general and
            // special cases
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;

            // p1, q1 and p2 are colinear and q2 lies on segment p1q1
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases
        }

    }

}
