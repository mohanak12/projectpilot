namespace ProjectPilot.Extras.LogParser
{
    public class LevelElement : ParsedElementBase
    {
        public override object Parse(string line)
        {
            string level = line;
            return level;
        }
    }
}
