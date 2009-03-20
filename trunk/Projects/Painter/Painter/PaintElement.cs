using System.Collections.Generic;
using System.Drawing;

namespace Painter
{
    public abstract class PaintElement
    {
        public PaintElement(Point point)
        {
            this.point = point;
        }

        public List<int> Parameters
        {
            get { return parameters; }
        }

        public Point Point
        {
            get { return point; }
        }

        public IDrawingProperties DrawingProperties
        {
            get { return drawingProperties; }
            set { drawingProperties = value; }
        }

        public void AddParameter (int parameter)
        {
            parameters.Add(parameter);
        }

        //public abstract void Draw(Graphics g, Pen pen);
        public abstract void Draw(IDrawingEngine engine);

        private IDrawingProperties drawingProperties;
        private Point point;
        private List<int> parameters = new List<int>();
    }
}