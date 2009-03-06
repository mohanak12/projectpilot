using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter
{
    class Line : PaintElement
    {
        public Line(Point point, Point endPoint) : base(point)
        {
            this.AddParameter(endPoint.X);
            this.AddParameter(endPoint.Y);
        }
    }
}
