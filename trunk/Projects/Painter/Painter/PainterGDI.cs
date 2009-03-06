using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Painter
{
    class PainterGDI : IPainter
    {
        public PainterGDI(Pen pen)
        {
            this.pen = pen;
        }

        public PainterGDI(Brush brush)
        {
            this.brush = brush;
        }

        public Pen Pen
        {
            get { return pen; }
            set { pen = value; }
        }

        public Brush Brush
        {
            get { return brush; }
            set { brush = value; }
        }

        public void Paint(Graphics g, PaintElement element)
        {
            if (element.GetType() == typeof (Rectangle))
            {
                g.DrawRectangle(pen, 
                                element.Point.X,
                                element.Point.Y, 
                                element.Parameters[0],
                                element.Parameters[1]);
            }
            else if (element.GetType() == typeof (Circle))
            {
                g.DrawEllipse(pen,
                              element.Point.X,
                              element.Point.Y,
                              element.Parameters[0],
                              element.Parameters[0]);
            }
            else if (element.GetType() == typeof(Line))
            {
                g.DrawLine(pen,
                              element.Point.X,
                              element.Point.Y,
                              element.Parameters[0],
                              element.Parameters[1]);
            }
        }

        public IList<PaintElement> Elements
        {
            get { return elements; }
        }

        public void AddElement(PaintElement element)
        {
            elements.Add(element);
        }
    
        private IList<PaintElement> elements = new List<PaintElement>();
        private Pen pen;
        private Brush brush;

        public void PaintAll(Graphics g)
        {
            foreach (PaintElement element in elements)
            {
                this.Paint(g,element);
            }
        }
    }
}
