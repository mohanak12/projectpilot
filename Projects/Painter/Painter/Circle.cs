using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter
{
    class Circle : PaintElement
    {
        public Circle(Point point, int radius) : base(point)
        {
            this.AddParameter(radius);
        }
    }
}
