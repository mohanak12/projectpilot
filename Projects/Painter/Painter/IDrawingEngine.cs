using System.Collections.Generic;

namespace Painter
{
    public interface IDrawingEngine
    {
        void Line(Point startPoint, Point endPoint);
        void Arc();
    }
}