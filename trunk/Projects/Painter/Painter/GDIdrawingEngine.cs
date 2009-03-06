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

        public void Line(Point startPoint, Point endPoint)
        {
            graphics.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
        }

        public void Arc()
        {
            //TODO
            throw new NotImplementedException();
            //graphics.DrawArc(pen,..);
        }

        private Graphics graphics;
        private Pen pen;
    }
}
