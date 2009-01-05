
using System;

namespace ProjectPilot.Extras.LogParser
{
    public class TimestampElement : ParsedElementBase
    {
        public override object Parse(string line)
        {
            DateTime dateTime = new DateTime();
            return dateTime;
        }
    }
}
