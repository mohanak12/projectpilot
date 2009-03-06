using System.Collections.Generic;
using System.Drawing;

namespace Painter
{
    public interface IPainter
    {

        void PaintAll(Graphics g);
        void Paint(Graphics g, PaintElement element);
        void AddElement(PaintElement element);

    }
}