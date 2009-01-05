namespace ProjectPilot.Extras.LogParser
{
    public class ThreadIdElement : ParsedElementBase
    {
        public override void Parse(string line)
        {
            Element = line;
        }
    }
}
