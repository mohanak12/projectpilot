using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter
{
    public class Window : PaintElement
    {
        public Window(Point point, int length, int width)
            : base(point)
        {
            this.AddParameter(length);
            this.AddParameter(width);
        }

        public override void Draw(IDrawingEngine engine)
        {
            Rectangle frame = new Rectangle(this.Point, this.Parameters[0], this.Parameters[1]);
            frame.Draw(engine);

            int x = this.Point.X;
            int y = this.Point.Y;
            int height = this.Parameters[0];
            int width = this.Parameters[1];

            engine.Line(new Point(x, y + width / 2), new Point(x + height, y + width / 2), this.DrawingProperties);
            engine.Line(new Point(x + height / 2 ,y), new Point(x + height / 2, y + width), this.DrawingProperties);
        }

    }
}
