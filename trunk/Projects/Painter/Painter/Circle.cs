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

        /*
        public override void Draw(Graphics g, Pen pen)
        {
            g.DrawEllipse(pen, this.Point.X, this.Point.Y, this.Parameters[0], this.Parameters[0]);
        
        }
        */

        
        public override void Draw(IDrawingEngine engine)
        {
            //Todo
        
            throw new NotImplementedException();
        }
         
    }
}
