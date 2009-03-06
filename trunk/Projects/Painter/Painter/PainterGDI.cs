using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Painter
{
    class PainterGDI : IPainter
    {
        public Graphics G { get; set; }
        //void PaintAll(Graphics g);
        //void Paint(Graphics g, PaintElement element);
        //void AddElement(PaintElement element);

        public PainterGDI(Graphics g, Pen pen)
        {
            this.g = g;
            this.pen = pen;
        }

        public PainterGDI(Graphics g, Brush brush)
        {
            this.g = g;
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

        public void Paint(IList<PaintElement> elements)
        {
            foreach (PaintElement element in elements)
            {
                //element.Draw(g, pen);
            }
        }

        private Graphics g;
        private Pen pen;
        private Brush brush;
    }
}
