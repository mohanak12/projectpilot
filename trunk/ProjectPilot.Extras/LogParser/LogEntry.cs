using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ProjectPilot.Extras.LogParser
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public class LogEntry
    {
        public IDictionary<string, string> LogItems
        {
            get { return logItems; }
        }

        //<LogPattern, value>
        private Dictionary<string, string> logItems = new Dictionary<string, string>();
    }
}
