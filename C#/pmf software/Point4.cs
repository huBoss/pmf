using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMF
{
    public class Point4
    {
        public Point3 point3 = new Point3();
        public float V = 0;

        public Point4()
        {
        }

        public Point4(Point3 pt3, float v)
        {
            point3 = pt3;
            V = v;
        }
    }
}
