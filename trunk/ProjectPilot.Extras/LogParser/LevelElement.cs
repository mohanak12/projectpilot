namespace ProjectPilot.Extras.LogParser
{
    public class LevelElement : ParsedElementBase
    {
        public override void Parse(string line)
        {
            Element = line;
        }
    }
}
