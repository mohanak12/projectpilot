using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Extras.LogParser
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    class DatePatternLayoutElement : ILog4NetPatternLayoutElement
    {
        public int Parse(string line, int startingIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
