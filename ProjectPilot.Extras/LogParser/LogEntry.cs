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
        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        public string ThreadId
        {
            get { return threadId; }
            set { threadId = value; }
        }

        public string Level
        {
            get { return level; }
            set { level = value; }
        }

        public string Undefined
        {
            get { return undefined; }
            set { undefined = value; }
        }

        private string date;
        private string time;
        private string threadId;
        private string level;
        private string undefined;
    }
}
