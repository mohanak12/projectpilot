using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Painter
{
    class Circle : PaintElement // : IDrawingEngine
    {
        public Circle(Point point, int radius) : base(point)
        {
            this.AddParameter(radius);
        }
       
        public override void Draw(IDrawingEngine engine)
        {
            engine.Arc(this.Point, this.Parameters[0] * 2, this.Parameters[0] * 2, 0, 360, this.DrawingProperties);
        }
         
    }
}
