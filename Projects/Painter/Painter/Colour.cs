using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Painter
{
    public class Colour
    {
        public Colour()
        {
            this.red = 0;
            this.green = 0;
            this.blue = 0;
        }

        public Colour(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public int Red
        {
            get { return red; }
            set { red = value; }
        }

        public int Green
        {
            get { return green; }
            set { green = value; }
        }

        public int Blue
        {
            get { return blue; }
            set { blue = value; }
        }

        private int red;
        private int green;
        private int blue;
    }
}
