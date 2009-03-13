using System;
using System.Collections.Generic;
using System.Drawing;
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

        public override void Draw(IDrawingEngine engine)
        {
            engine.Line(this.Point, new Point(this.Parameters[0], this.Parameters[1]));
        }
    }
}
