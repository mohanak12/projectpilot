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

        private string timePattern;
        private CultureInfo cultureToUse;
    }
}
