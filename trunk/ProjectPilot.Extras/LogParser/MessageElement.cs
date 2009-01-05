using ProjectPilot.Extras.LogParser;

namespace ProjectPilot.Extras.LogParser
{
    public class MessageElement : ParsedElementBase
    {
        public override object Parse(string line)
        {
            string message = line;
            return message;
        }
    }
}
