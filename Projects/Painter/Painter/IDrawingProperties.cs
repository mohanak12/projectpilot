using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Painter
{
    public interface IDrawingProperties
    {
        Colour Colour
        {
            get;
            set;
        }

        int Width
        {
            get;
            set;
        }
    }
}
