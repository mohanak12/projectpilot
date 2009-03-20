using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter
{
    public class GDIDrawingProperties : IDrawingProperties
    {
        public GDIDrawingProperties()
        {
            this.Colour = new Colour();
            this.Width = 5;
        }

        public GDIDrawingProperties(Colour colour, int width)
        {
            this.colour = colour;
            this.width = width;
        }

        public Colour Colour
        {
            get { return colour; }
            set { colour = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
     
        private Colour colour;
        private int width;
    }
}
