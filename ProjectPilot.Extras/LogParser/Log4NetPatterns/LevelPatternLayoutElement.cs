using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Extras.LogParser.Log4NetPatterns
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public class LevelPatternLayoutElement : ConversionPatternBase
    {
        public override int Parse(string line, int startingIndex)
        {
            throw new System.NotImplementedException();
        }

        public string Regex
        {
            get { return regex; }
        }

        private string regex = @"[A-Z]{3,5}";
    }
}