using System.Collections.Generic;

namespace Painter
{
    public interface IDrawingEngine
    {
        void Line(Point startPoint, Point endPoint, IDrawingProperties properties);
        void Arc(Point point, int height, int width, int startAngle, int sweepAngle, IDrawingProperties properties);
    }
}