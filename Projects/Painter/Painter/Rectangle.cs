using System;
using System.Collections.Generic;
using System.Drawing;
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

        /*
        public override void Draw(Graphics g, Pen pen)
        {
            g.DrawRectangle(pen, this.Point.X, this.Point.Y, this.Parameters[0], this.Parameters[1]);
        } 
        */

        public override void Draw(IDrawingEngine engine)
        {
            //this.Parameters[0] - width
            //this.Parameters[1] - height

            engine.Line(this.Point, new Point(this.Point.X, this.Point.Y + this.Parameters[1]));

            engine.Line(this.Point, new Point(this.Point.X + this.Parameters[0], this.Point.Y));

            engine.Line(
                new Point(this.Point.X + this.Parameters[0], this.Point.Y),
                new Point(this.Point.X + this.Parameters[0], this.Point.Y + this.Parameters[1]));

            engine.Line(
                new Point(this.Point.X, this.Point.Y + this.Parameters[1]),
                new Point(this.Point.X + this.Parameters[0], this.Point.Y + this.Parameters[1]));
        }
    }
}