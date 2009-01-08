using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Extras.LogParser
{
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
            string filterLevel)
        {
            this.filterTimestampStart = filterTimestampStart;
            this.filterTimestampEnd = filterTimestampEnd;
            this.filterThreadId = filterThreadId;
            this.filterLevel = filterLevel;
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

        private DateTime? filterTimestampStart;
        private DateTime? filterTimestampEnd;
        private string filterThreadId;
        private string filterLevel;
    }
}
