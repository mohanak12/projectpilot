namespace ProjectPilot.Extras.LogParser
{
    public class ThreadIdElement : ParsedElementBase
    {
        public override object Parse(string line)
        {
            string threadId = line;
            return threadId;
        }
    }
}
