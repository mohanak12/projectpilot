using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ProjectPilot.Extras.LogParser
{
    public class TimestampElement : ParsedElementBase
    {
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public CultureInfo CultureToUse
        {
            set { cultureToUse = value; }
        }

        public override void Parse(string line)
        {
            DateTime dateTime = DateTime.ParseExact(line, timePattern, cultureToUse);
            Element = dateTime;
        }
        
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public string TimePattern
        {
            set { timePattern = value; }
        }

        public override string ToString()
        {
            DateTime dateTime = (DateTime) Element;
            return base.ToString() + "," + dateTime.Millisecond;
        }

        private string timePattern;
        private CultureInfo cultureToUse;
    }
}
