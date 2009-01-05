using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Extras.LogParser
{
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ndc")]
    public class NdcElement : ParsedElementBase
    {
        public override object Parse(string line)
        {
            string ndc = line;
            return ndc;
        }
    }
}
