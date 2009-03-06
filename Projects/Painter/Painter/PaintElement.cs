using System.Collections.Generic;

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

        public void AddParameter (int parameter)
        {
            parameters.Add(parameter);
        }

        private Point point;
        private List<int> parameters = new List<int>();
    }
}