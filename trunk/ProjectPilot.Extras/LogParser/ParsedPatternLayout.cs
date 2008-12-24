using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Extras.LogParser
{
    public class ParsedPatternLayout
    {
        // %timestamp [%thread] %level %logger %ndc - %message%newline
        // PatternPatternLayoutElement (PatternPatternLayoutElementType.TimeStamp)
        // LiteralPatternLayoutElement (" [")
        // ....


        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private List<ILog4NetPatternLayoutElement> elements = new List<ILog4NetPatternLayoutElement>();
    }
}