using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using log4net;
using MbUnit.Framework;
using ProjectPilot.Extras.LogParser;
using ProjectPilot.Extras.LogParser.Log4NetPatterns;

namespace ProjectPilot.Tests.Extras
{
    [TestFixture]
    public class LogParserTests
    {
        [Test]
        public void SimpleLineParsing()
        {
            LogCollection lineParse = new LogCollection('|', "Time|ThreadId");

            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]");

            Assert.AreEqual(2, lineParse.ElementsPattern.Count);
            Assert.AreEqual("Time", lineParse.ElementsPattern[0]);
            Assert.AreEqual("ThreadId", lineParse.ElementsPattern[1]);

            Assert.AreEqual(1, lineParse.ElementsLog.Count);
            Assert.AreEqual(2, lineParse.ElementsLog[0].Elements.Count);

            Assert.AreEqual(typeof(TimestampElement).FullName, lineParse.ElementsLog[0].Elements[0].GetType().FullName);
            Assert.AreEqual(typeof(ThreadIdElement).FullName, lineParse.ElementsLog[0].Elements[1].GetType().FullName);

            DateTime expectedTime = DateTime.ParseExact("2008-12-22 09:16:02,734", "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.CurrentCulture);
            Assert.AreEqual(
                expectedTime,
                ((ParsedElementBase)lineParse.ElementsLog[0].Elements[0]).Element);

            Assert.AreEqual(
                "[4904]",
                ((ParsedElementBase)lineParse.ElementsLog[0].Elements[1]).Element);
        }

        [Test]
        public void ComplexLineParsing()
        {
            LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Ndc");
            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]|INFO|Hsl.UniversalHost.|");

            Assert.AreEqual(4, lineParse.ElementsPattern.Count);
            Assert.AreEqual("Time", lineParse.ElementsPattern[0]);
            Assert.AreEqual("ThreadId", lineParse.ElementsPattern[1]);
            Assert.AreEqual("Level", lineParse.ElementsPattern[2]);
            Assert.AreEqual("Ndc", lineParse.ElementsPattern[3]);

            Assert.AreEqual(1, lineParse.ElementsLog.Count);
            Assert.AreEqual(4, lineParse.ElementsLog[0].Elements.Count);

            DateTime expectedTime = DateTime.ParseExact("2008-12-22 09:16:02,734", "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.CurrentCulture);

            Assert.AreEqual(
                expectedTime,
                ((ParsedElementBase)lineParse.ElementsLog[0].Elements[0]).Element);

            Assert.AreEqual("[4904]", ((ParsedElementBase)lineParse.ElementsLog[0].Elements[1]).Element);
            Assert.AreEqual("INFO", ((ParsedElementBase)lineParse.ElementsLog[0].Elements[2]).Element);

            Assert.AreEqual(
            "Hsl.UniversalHost.|",
            ((ParsedElementBase)lineParse.ElementsLog[0].Elements[3]).Element);
        }

        [Test]
        public void DoubleLineParsing()
        {
            LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Ndc");
            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]|INFO|Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components.");
            lineParse.ParseLogLine(@" 2008-12-22 09:16:02,734|[4904]FAIL|Hsl3");

            Assert.AreEqual(
            "Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components. 2008-12-22 09:16:02,734|[4904]FAIL|Hsl3",
            ((ParsedElementBase)lineParse.ElementsLog[0].Elements[3]).Element);
        }

        [Test]
        public void ThreeElementsParsing()
        {
            LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Ndc");
            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]|INFO|Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components.");
            lineParse.ParseLogLine(@"2008-12-22 09:16:05,577|[3152]|INFO|Hsl.UniversalHost.Core.ActiveComponent [Start Component SmsCenter] - Component 'SmsCenter' started.");
            lineParse.ParseLogLine(@"!T|ES|T!");
            lineParse.ParseLogLine(@"2008-12-22 09:37:59,977|[3368]|INFO|Hsl.Sms.Emi.Connection [Listen(1)] - Closing connection '1'.");

            Assert.AreEqual(3, lineParse.ElementsLog.Count);

            Assert.AreEqual(
            "Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components.",
            ((ParsedElementBase)lineParse.ElementsLog[0].Elements[3]).Element);

            Assert.AreEqual(
            "Hsl.UniversalHost.Core.ActiveComponent [Start Component SmsCenter] - Component 'SmsCenter' started.!T|ES|T!",
            ((ParsedElementBase)lineParse.ElementsLog[1].Elements[3]).Element);

            Assert.AreEqual(
            "Hsl.Sms.Emi.Connection [Listen(1)] - Closing connection '1'.",
            ((ParsedElementBase)lineParse.ElementsLog[2].Elements[3]).Element);
        }

        [Test]
        public void ParsingFromLogFile()
        {
            using (Stream fileStream = File.OpenRead(@"..\..\..\Data\Samples\TestLogParser.log"))
            {
                LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Ndc");

                lineParse.ParseLogFile(fileStream);

                Assert.AreEqual(15, lineParse.ElementsLog.Count);
            }
        }

        [Test]
        public void ParsingDifferentTimePatterns()
        {
            CultureInfo cultureToUse = CultureInfo.InvariantCulture;
            string timePattern = "yyyy-MM-dd HH:mm:ss.fff";
            LogCollection lineParse = new LogCollection('|', "Time|ThreadId", timePattern, cultureToUse);

            lineParse.ParseLogLine(@"2008-12-22 09:16:02.734|[4904]");

            DateTime expectedTime = DateTime.ParseExact("2008-12-22 09:16:02.734", timePattern, CultureInfo.CurrentCulture);
            Assert.AreEqual(
                expectedTime,
                ((ParsedElementBase)lineParse.ElementsLog[0].Elements[0]).Element);

            timePattern = "MM+dd+yy HH:mm:ss,fff";
            LogCollection lineParse2 = new LogCollection('|', "Time|ThreadId", timePattern, cultureToUse);

            lineParse2.ParseLogLine(@"12+22+08 09:16:02,734|[4904]");

            expectedTime = DateTime.ParseExact("12+22+08 09:16:02,734", timePattern, CultureInfo.CurrentCulture);
            Assert.AreEqual(
                expectedTime,
                ((ParsedElementBase)lineParse2.ElementsLog[0].Elements[0]).Element);

            timePattern = "MMddyy";
            LogCollection lineParse3 = new LogCollection('|', "Time|ThreadId", timePattern, cultureToUse);

            lineParse3.ParseLogLine(@"122208|[4904]");

            expectedTime = DateTime.ParseExact("122208", timePattern, CultureInfo.CurrentCulture);
            Assert.AreEqual(
                expectedTime,
                ((ParsedElementBase)lineParse3.ElementsLog[0].Elements[0]).Element);
        }

        [Test]
        [Row(null, null, null, null, null, 15)]
        [Row(null, null, null, null, 10, 10)]
        [Row(null, null, null, "WARN", null, 4)]
        [Row(null, null, "[5448]", null, null, 3)]
        [Row(null, null, "[2572]", "WARN", null, 2)]
        [Row(null, null, "[3688]", "INFO", null, 6)]
        [Row("2008-12-22 09:58:45,481", "2008-12-22 09:58:47,495", null, null, null, 1)]
        [Row("2008-12-22 09:58:45,480", "2008-12-22 10:07:49,202", null, null, null, 8)]
        [Row("2008-12-22 10:04:37,895", "2008-12-22 10:04:37,895", null, null, null, 1)]
        [Row("2008-12-22 09:59:33,210", "2008-12-22 10:07:49,202", "[908]", "INFO", null, 1)]
        [Row("2008-12-22 10:07:46,359", "2008-12-22 10:07:47,359", null, null, 1, 1)]  
        public void ParsingFromLogFileWithFilter(string dateTimeStart, string dateTimeEnd, string threadId, string filterLevel, int? filterNumberOfLogItems, int expectedElementsLogCount)
        {
            using (Stream fileStream = File.OpenRead(@"..\..\..\Data\Samples\TestLogParser.log"))
            {
                LogParserFilter filter = new LogParserFilter();
                if (!string.IsNullOrEmpty(dateTimeStart) && !string.IsNullOrEmpty(dateTimeEnd))
                {
                    filter.FilterTimestampStart = DateTime.ParseExact(dateTimeStart, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.CurrentCulture);
                    filter.FilterTimestampEnd = DateTime.ParseExact(dateTimeEnd, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.CurrentCulture);
                }

                filter.FilterThreadId = threadId;
                filter.FilterLevel = filterLevel;
                filter.FilterNumberOfLogItems = filterNumberOfLogItems;

                LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Ndc");
                lineParse.ParseFilter = filter;
                lineParse.ParseLogFile(fileStream);

                Assert.AreEqual(expectedElementsLogCount, lineParse.ElementsLog.Count);
            }
        }

        [Test]
        [Row(null, null, null, 15)]
        [Row(null, null, 3, 3)]
        [Row(2, 2, null, 1)]
        [Row(3, 6, 5, 4)]
        [Row(3, 6, 2, 2)]
        [Row(null, 6, 2, 2)]
        [Row(null, 6, 9, 7)]
        [Row(2, null, 9, 9)]
        [Row(10, null, 9, 6)]
        [Row(10, 8, 2, 0)]
        [Row(-10, -9, 2, 0)]
        [Row(-10, 2, 2, 2)]
        [Row(-10, 2, -2, 0)]
        public void TestingLogIndexBoundsFilter(int? startLogIndex, int? endLogIndex, int? filterNumberOfLogItems, int expectedElementsLogCount)
        {
            using (Stream fileStream = File.OpenRead(@"..\..\..\Data\Samples\TestLogParser.log"))
            {
                LogParserFilter filter = new LogParserFilter();
                filter.StartLogIndex = startLogIndex;
                filter.EndLogIndex = endLogIndex;
                filter.FilterNumberOfLogItems = filterNumberOfLogItems;

                LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Ndc");
                lineParse.ParseFilter = filter;
                lineParse.ParseLogFile(fileStream);

                Assert.AreEqual(expectedElementsLogCount, lineParse.ElementsLog.Count);
            }
        }

        [Test]
        [Row(null, 15)]
        [Row("[Listen(1)]", 4)]
        [Row("[(null)]", 10)]
        [Row("null", 0)]
        [Row("Test", 0)]
        public void TestingSearchWholeWordOnlyFilter(string matchWholeWordOnly, int expectedElementsLogCount)
        {
            using (Stream fileStream = File.OpenRead(@"..\..\..\Data\Samples\TestLogParser.log"))
            {
                LogParserFilter filter = new LogParserFilter();
                filter.MatchWholeWordOnly = matchWholeWordOnly;

                LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Ndc");
                lineParse.ParseFilter = filter;
                lineParse.ParseLogFile(fileStream);

                Assert.AreEqual(expectedElementsLogCount, lineParse.ElementsLog.Count);
            }
        }

        [Test]
        [Row(null, 15)]
        [Row("End", 7)]
        [Row("null", 10)]
        [Row("TeSt", 10)]
        [Row("TeSt123", 0)]
        public void TestingSearchMatchFilter(string matchCase, int expectedElementsLogCount)
        {
            using (Stream fileStream = File.OpenRead(@"..\..\..\Data\Samples\TestLogParser.log"))
            {
                LogParserFilter filter = new LogParserFilter();
                filter.Match = matchCase;

                LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Ndc");
                lineParse.ParseFilter = filter;
                lineParse.ParseLogFile(fileStream);

                Assert.AreEqual(expectedElementsLogCount, lineParse.ElementsLog.Count);
            }
        }

        [Test]
        public void LogCountModeFilterTesting()
        {
            using (Stream fileStream = File.OpenRead(@"..\..\..\Data\Samples\TestLogParser.log"))
            {
                LogParserFilter filter = new LogParserFilter();
                filter.LogCountMode = true;

                LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Ndc");
                lineParse.ParseFilter = filter;
                lineParse.ParseLogFile(fileStream);

                Assert.AreEqual(15, lineParse.NumberOfLogItems);

                //Results: 13,2 KB file take 2,685  seconds  (around 50 lines - 14 log entries)
                //Results: 14,2 KB file take 3,081  seconds  (around 65 lines - 14 log entries)
                //Results: 9,98 MB file take 4,770  seconds  (around 14.500 lines  - 2530 log entries)
                //Results: 35,2 MB file take 8,263  seconds  (around 50.000 lines  - 8740 log entries)
                //Results: 71,5 MB file take 12,429 seconds  (around 100.000 lines - 17.710 log entries)
                //Results:  142 MB file take 21,642 seconds  (around 200.000 lines - 35.190 log entries)
            }
        }

        [Test]
        [Row(-1, 10472, 0)]
        [Row(0, 1500, 1)]
        [Row(8052294, 10000294, 490)]
        [Row(10452294, 10472300, 2)]
        [Row(10452294, 10472294, 2)]
        [Row(10452294, 10472293, 1)]
        public void ParsingPartOf10MBLogFile(int readIndexStart, long readIndexEnd, int expectedElementsLogCount)
        {
            using (Stream fileStream = File.OpenRead(@"..\..\..\Data\Samples\SSM+2009-01-08.log.28"))
            {   // Size of file = 10472294
                LogCollection lineParse = new LogCollection('|', "Time|Level|Ndc");

                LogParserFilter filter = new LogParserFilter();
                filter.ReadIndexStart = readIndexStart;
                filter.ReadIndexEnd = readIndexEnd;

                lineParse.ParseFilter = filter;
                lineParse.ParseLogFile(fileStream);

                Assert.AreEqual(expectedElementsLogCount, lineParse.ElementsLog.Count);
            }
        }

        [Test]
        public void ParsingLog4NetFileNamespaceTesting()
        {
            using (Stream fileStream = File.OpenRead(@"..\..\..\Data\Samples\ProjectPilot.Tests.log.2"))
            {   
                LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Namespace|Message");

                lineParse.ParseLogFile(fileStream);

                Assert.AreEqual(61, lineParse.ElementsLog.Count);
                Assert.AreEqual("Project.MyProject", ((NamespaceElement)lineParse.ElementsLog[0].Elements[3]).Element);
                Assert.AreEqual("Builds.VSSolutionBrowsing.VSProject", ((NamespaceElement)lineParse.ElementsLog[60].Elements[3]).Element);
                Assert.AreEqual("Builds.VSSolutionBrowsing.VSSolution", ((NamespaceElement)lineParse.ElementsLog[50].Elements[3]).Element);
            }
        }

        //        [Test]   still to implement!   - Marko
        //        public void TestAllFilters() {}

        [FixtureSetUp]
        public void FixtureSetup()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}