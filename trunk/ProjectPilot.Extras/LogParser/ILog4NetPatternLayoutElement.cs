namespace ProjectPilot.Extras.LogParser
{
    public interface ILog4NetPatternLayoutElement
    {
        int Parse(string line, int startingIndex);
    }
}