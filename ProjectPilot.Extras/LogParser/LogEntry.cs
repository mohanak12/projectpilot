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

        public IList<string> Elements
        {
            get { return elements; }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        private List<string> elements = new List<string>();
        private Dictionary<string, string> logItems = new Dictionary<string, string>();
    }
}
