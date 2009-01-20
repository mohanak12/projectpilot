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
            int? filterNumberOfLogItems,
            int? startLogIndex,
            int? endLogIndex,
            string matchWholeWordOnly,
            string matchCase)
        {
            this.filterTimestampStart = filterTimestampStart;
            this.filterTimestampEnd = filterTimestampEnd;
            this.filterThreadId = filterThreadId;
            this.filterLevel = filterLevel;
            this.startLogIndex = startLogIndex;
            this.endLogIndex = endLogIndex;
            this.filterNumberOfLogItems = filterNumberOfLogItems;
            this.matchWholeWordOnly = matchWholeWordOnly;
            this.matchCase = matchCase;
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

        public int? EndLogIndex
        {
            get { return endLogIndex; }
            set { endLogIndex = value; }
        }

        public int? StartLogIndex
        {
            get { return startLogIndex; }
            set { startLogIndex = value; }
        }

        public string MatchCase
        {
            get { return matchCase; }
            set { matchCase = value; }
        }

        public string MatchWholeWordOnly
        {
            get { return matchWholeWordOnly; }
            set { matchWholeWordOnly = value; }
        }

        private int? endLogIndex;
        private DateTime? filterTimestampStart;
        private DateTime? filterTimestampEnd;
        private string filterThreadId;
        private string filterLevel;
        private int? filterNumberOfLogItems;
        private string matchCase;
        private string matchWholeWordOnly;
        private int? startLogIndex;
    }
}
