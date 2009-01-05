using ProjectPilot.Extras.LogParser;

namespace ProjectPilot.Extras.LogParser
{
    public class MessageElement : ParsedElementBase
    {
        public override void Parse(string line)
        {
            Element = line;
        }
    }
}
