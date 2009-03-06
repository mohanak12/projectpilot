using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter
{
    class Rectangle : PaintElement
    {
        public Rectangle(Point point, int length, int width) : base(point)
        {
             this.AddParameter(length);
             this.AddParameter(width);
        }
    }
}
