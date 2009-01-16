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
        [Row(null, null, null, null, 0, 15)]
        [Row(null, null, null, null, 10, 10)]
        [Row(null, null, null, "WARN", 0, 4)]
        [Row(null, null, "[5448]", null, 0, 3)]
        [Row(null, null, "[2572]", "WARN", 0, 2)]
        [Row(null, null, "[3688]", "INFO", 0, 6)]
        [Row("2008-12-22 09:58:45,481", "2008-12-22 09:58:47,495", null, null, 0, 1)]
        [Row("2008-12-22 09:58:45,480", "2008-12-22 10:07:49,202", null, null, 0, 8)]
        [Row("2008-12-22 10:04:37,895", "2008-12-22 10:04:37,895", null, null, 0, 1)]
        [Row("2008-12-22 09:59:33,210", "2008-12-22 10:07:49,202", "[908]", "INFO", 1, 1)]
        [Row("2008-12-22 10:07:46,359", "2008-12-22 10:07:47,359", null, null, 0, 1)]
        public void ParsingFromLogFileWithFilter(string dateTimeStart, string dateTimeEnd, string threadId, string filterLevel, int filterNumberOfLogItems, int expectedElementsLogCount)
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

//        [Test]
//        public void Parsing10MBLogFile()
//        {
//            using (Stream fileStream = File.OpenRead(@"C:\SSM+2009-01-08.log.28"))
//            {
//                LogCollection lineParse = new LogCollection('|', "Time|Level|Ndc");
//                
//                LogParserFilter filter = new LogParserFilter();
//                filter.FilterLevel = "WARN";
//                lineParse.ParseFilter = filter;
//                lineParse.ParseLogFile(fileStream);
//                
//                Assert.AreEqual(272, lineParse.ElementsLog.Count);
//            }
//        }

        [FixtureSetUp]
        public void FixtureSetup()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}