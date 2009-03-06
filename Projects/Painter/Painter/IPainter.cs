using System.Collections.Generic;

namespace Painter
{
    public interface IPainter
    {
        void Paint(IList<PaintElement> elements);
    }
}