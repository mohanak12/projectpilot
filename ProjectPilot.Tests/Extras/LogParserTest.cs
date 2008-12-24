using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using ProjectPilot.Extras.LogParser;

namespace ProjectPilot.Tests.Extras
{
    [TestFixture]
    public class LogParserTest
    {
        [Test, Pending]
        public void TestLogParser()
        {
//            Regex regexDate = new Regex(@"^\d{4}-\d{2}-\d{2}$");
//            Regex regexThreadId = new Regex(@"\[\d+\]");
//            //PatternPatternLayoutElementTypeRegex regexLevel = new Regex(@"\[\d+\]");
//            Regex regexTime = new Regex(@"^\d{2}:\d{2}:\d{2},\d{3}$");
//
//            Dictionary<string, Regex> pattern = new Dictionary<string, Regex>();
//            pattern.Add("Date", regexDate);
//            pattern.Add("Time", regexTime);
//            pattern.Add("ThreadId", regexThreadId);
//            
//            Stream stream = File.OpenRead(@"..\..\..\Data\Samples\TestServer.log");
//            LogCollection parseCollection = new LogCollection();
//
//            parseCollection.ParseLogFile(stream, pattern);

            ParsedPatternLayout patternParse = new ParsedPatternLayout();
              patternParse.ParsePattern("%timestamp [%thread] %level %logger %ndc - %message%newline");
           // patternParse.ParsePattern("%-6timestamp");
        }
    }
}