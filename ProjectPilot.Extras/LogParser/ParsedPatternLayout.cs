using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace ProjectPilot.Extras.LogParser
{
    public class ParsedPatternLayout
    {
        // %timestamp [%thread] %level %logger %ndc - %message%newline
        // PatternPatternLayoutElement (PatternPatternLayoutElementType.TimeStamp)
        // LiteralPatternLayoutElement (" [")
        // ....\

        public void ParsePattern(string line)
        {
//            int index, 
              int count = 0;
              string patternElement = "", literal = "";
//            index = line.IndexOf(' ');
           //element = line.Substring(line.IndexOf('%'), line.IndexOf(' '));

            bool test = line.Contains("timestamp");

            Regex regex= new Regex(@"%-?[0-9]*\.?[0-9]*(?<pattern>[a-zA-Z]+)");
           //Regex regex1 = new Regex(@"%-(?<min>[0-9]*)\.(?<max>[0-9]*)(?<pattern>[a-zA-Z]+)");

            string[] parseArray = line.Split(' ');

            foreach (string pattern in parseArray)
            {
                if(regex.IsMatch(pattern))
                {
                    Match match = regex.Match(pattern);
                    count++;
                }
            }


            for (int n=0; n<line.Length; n++)
            {
                    if(line[n] == '%')
                    {
                      patternElement = line.Substring(line.IndexOf('%', n), line.IndexOf(' '));
                      n = n + patternElement.Length;
                    }
                    if(line[n] != '%')
                    {
                        literal = literal + line[n];
                    }
            }
        }

        //if(patternElement)

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private List<ILog4NetPatternLayoutElement> elements = new List<ILog4NetPatternLayoutElement>();

        //private string line = @"%timestamp [%thread] %level %logger %ndc - %message%newline";
//        private string line1 = @"%-6timestamp [%15.15thread] %-5level %30.30logger %ndc - %message%newline";
//        private string patternElement = "%-6timestamp";
//        private string patternElement1 = "";
    }
}