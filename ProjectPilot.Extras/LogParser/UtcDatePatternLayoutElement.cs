using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Extras.LogParser
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public class UtcDatePatternLayoutElement : ConversionPatternBase
    {
        public UtcDatePatternLayoutElement()
        {
        }

        public override int Parse(string line, int startingIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
