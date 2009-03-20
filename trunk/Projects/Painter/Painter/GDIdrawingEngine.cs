using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Painter
{
    class GDIdrawingEngine : IDrawingEngine
    {
        public GDIdrawingEngine(Graphics graphics)
        {
            this.graphics = graphics;
            this.pen = new Pen(Color.Black, 5);  //Default Pen
        }

        public void Line(Point startPoint, Point endPoint, IDrawingProperties properties)
        {
            if (properties != null)
            {
                this.pen.Color = Color.FromArgb(properties.Colour.Red, properties.Colour.Green, properties.Colour.Blue);
                this.pen.Width = properties.Width;
            }
            graphics.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
        }

        public void Arc(Point point, int height, int width, int startAngle, int sweepAngle, IDrawingProperties properties)
        {
            if(properties != null)
            {
                this.pen.Color = Color.FromArgb(properties.Colour.Red, properties.Colour.Green, properties.Colour.Blue);
                this.pen.Width = properties.Width;
            }
            graphics.DrawArc(pen, point.X, point.Y, width, height, startAngle, sweepAngle);
        }

        private Graphics graphics;
        private Pen pen;
    }
}
