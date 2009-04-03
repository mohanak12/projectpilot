using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter
{
    public class House : PaintElement
    {
        public House(Point point, int length, int width) : base(point)
        {
            this.AddParameter(length);
            this.AddParameter(width);
        }

        public override void Draw(IDrawingEngine engine)
        {
            int x = this.Point.X;
            int y = this.Point.Y;
            int height = this.Parameters[0];
            int width = this.Parameters[1];

            engine.Line(new Point(x, y+(y/8)), new Point(x + width/2, y), this.DrawingProperties);
            engine.Line(new Point(x + width / 2, y), new Point(x + width, y + y/8), this.DrawingProperties);

            Rectangle house = new Rectangle(new Point(x,  y+(y/8)), width, height);
            house.Draw(engine);

            Window window = new Window(new Point(x + x/5, y + y/5), height/5, width/5);
            window.Draw(engine);

            Window window1 = new Window(new Point(x - x / 5 + width, y + y / 5), height / 5, width / 5);
            window1.Draw(engine);
        }
    }
}
