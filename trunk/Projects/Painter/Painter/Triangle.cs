using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter
{
    class Triangle : PaintElement
    {
        public Triangle(Point pointA, Point pointB, Point pointC): base(pointA)
        {
            this.AddParameter(pointB.X);
            this.AddParameter(pointB.Y);
            this.AddParameter(pointC.X);
            this.AddParameter(pointC.Y);
        }

        public override void Draw(IDrawingEngine engine)
        {
            engine.Line(this.Point, new Point(this.Parameters[0], this.Parameters[1]));  // PointA - PointB
            engine.Line(this.Point, new Point(this.Parameters[2], this.Parameters[3]));  // PointA - PointC
            engine.Line(new Point(this.Parameters[0], this.Parameters[1]), 
                        new Point(this.Parameters[2], this.Parameters[3])); //PointB - PointC
        }
    }
}
