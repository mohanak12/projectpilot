using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Extras.LogParser
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
