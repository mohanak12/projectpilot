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
        public void ParseEntry(string[] patternArray, string line)
        {
            string[] parseArray = line.Split(' ');
            for (int i = 0; i < patternArray.Count(); i++)
            {
                logItems.Add(patternArray[i],parseArray[i]);
            }
        }

        public IDictionary<string, string> LogItems
        {
            get { return logItems; }
        }

        //<LogPattern, value>
        private Dictionary<string, string> logItems = new Dictionary<string, string>();
    }
}
