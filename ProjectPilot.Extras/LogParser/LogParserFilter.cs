using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Extras.LogParser
{
    /// <summary>
    /// Contains filter information for LogParser
    /// Default: No filter
    /// </summary>
    public class LogParserFilter
    {
        public LogParserFilter()
        {
            //Filter is off.
        }

        public LogParserFilter(
            DateTime filterTimestampStart,
            DateTime filterTimestampEnd,
            string filterThreadId,
            string filterLevel,
            int filterNumberOfLogItems)
        {
            this.filterTimestampStart = filterTimestampStart;
            this.filterTimestampEnd = filterTimestampEnd;
            this.filterThreadId = filterThreadId;
            this.filterLevel = filterLevel;
            this.filterNumberOfLogItems = filterNumberOfLogItems;
        }

        public string FilterLevel
        {
            get { return filterLevel; }
            set { filterLevel = value; }
        }

        public DateTime? FilterTimestampStart
        {
            get { return filterTimestampStart; }
            set { filterTimestampStart = value; }
        }

        public DateTime? FilterTimestampEnd
        {
            get { return filterTimestampEnd; }
            set { filterTimestampEnd = value; }
        }

        public string FilterThreadId
        {
            get { return filterThreadId; }
            set { filterThreadId = value; }
        }

        public int? FilterNumberOfLogItems
        {
            get { return filterNumberOfLogItems; }
            set { filterNumberOfLogItems = value; }
        }

        private DateTime? filterTimestampStart;
        private DateTime? filterTimestampEnd;
        private string filterThreadId;
        private string filterLevel;
        private int? filterNumberOfLogItems;
    }
}
