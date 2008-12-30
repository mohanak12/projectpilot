using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Extras.LogParser.Log4NetPatterns
{
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ndc")]
    public class NdcPatternLayoutElement : ConversionPatternBase
    {
        public override int Parse(string line, int startingIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}