using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ProjectPilot.Extras.LogParser
{
    public class LogEntry
    {
        public IList<object> Elements
        {
            get { return elements; }
        }

        private List<object> elements = new List<object>();
    }
}
