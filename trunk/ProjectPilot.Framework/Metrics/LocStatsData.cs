using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Framework.Metrics
{
    public class LocStatsData
    {
        public LocStatsData(int sloc, int cloc, int eloc)
        {
            this.sloc = sloc;
            this.cloc = cloc;
            this.eloc = eloc;
        }

        public int Cloc
        {
            get { return cloc; }
            set { cloc = value; }
        }

        public int Eloc
        {
            get { return eloc; }
            set { eloc = value; }
        }

        public int Sloc
        {
            get { return sloc; }
            set { sloc = value; }
        }

        private int cloc;
        private int eloc;
        private int sloc;
    }
}
