using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Extras.LogParser
{
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ndc")]
    public class NdcElement : ParsedElementBase
    {
        public override void Parse(string line)
        {
            Element = line;
        }
    }
}
