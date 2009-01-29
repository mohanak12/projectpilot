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

        public string MatchWholeWordOnly
        {
            get { return matchWholeWordOnly; }
            set { matchWholeWordOnly = value; }
        }

        public string MatchCase
        {
            get { return matchCase; }
            set { matchCase = value; }
        }

        public long? ReadIndexEnd
        {
            get { return readIndexEnd; }
            set { readIndexEnd = value; }
        }

        public long? ReadIndexStart
        {
            get { return readIndexStart; }
            set { readIndexStart = value; }
        }

        public bool LogCountMode
        {
            get { return logCountMode; }
            set { logCountMode = value; }
        }

        private int? endLogIndex;
        private DateTime? filterTimestampStart;
        private DateTime? filterTimestampEnd;
        private string filterThreadId;
        private string filterLevel;
        private int? filterNumberOfLogItems;
        private bool logCountMode;
        private string matchCase;
        private string matchWholeWordOnly;
        private long? readIndexEnd;
        private long? readIndexStart;
        private int? startLogIndex;
    }
}
