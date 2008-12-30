using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Extras.LogParser.Log4NetPatterns
{
    [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Username")]
    public class UsernamePatternLayoutElement : ConversionPatternBase
    {
        public override int Parse(string line, int startingIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}