using System;
using System.Globalization;

namespace ProjectPilot.Extras.LogParser
{
    public class TimestampElement : ParsedElementBase
    {
        public override void Parse(string line)
        {
            DateTime dateTime = DateTime.ParseExact(line, timePattern, CultureInfo.CurrentCulture);
           // DateTime dateTime = new DateTime();
            Element = dateTime;
        }

        private string timePattern = "yyyy-MM-dd HH:mm:ss,fff";
    }
}
