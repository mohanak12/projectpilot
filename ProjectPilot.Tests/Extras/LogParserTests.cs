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
        [Test, Pending]
        public void SimpleLineParsing()
        {
            LogCollection lineParse = new LogCollection('|', "Time|ThreadId");
            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]");
            
            Assert.AreEqual(2, lineParse.ElementsPattern.Count);
            Assert.AreEqual("Time", lineParse.ElementsPattern[0]);
            Assert.AreEqual("ThreadId", lineParse.ElementsPattern[1]);

            Assert.AreEqual(1, lineParse.ElementsLog.Count);
            Assert.AreEqual(2, lineParse.ElementsLog[0].Elements.Count);
            DateTime expectedTime = DateTime.ParseExact("2008-12-22 09:16:02,734", "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.CurrentCulture);
            Assert.AreEqual(
                expectedTime, 
                (DateTime)lineParse.ElementsLog[0].Elements[0]);
            Assert.AreEqual("[4904]", lineParse.ElementsLog[0].Elements[1]);
        }

        [Test]
        public void ComplexLineParsing()
        {
            LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Unsorted");
            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]|INFO|Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components.");

            Assert.AreEqual(4, lineParse.ElementsPattern.Count);
            Assert.AreEqual("Time", lineParse.ElementsPattern[0]);
            Assert.AreEqual("ThreadId", lineParse.ElementsPattern[1]);
            Assert.AreEqual("Level", lineParse.ElementsPattern[2]);
            Assert.AreEqual("Unsorted", lineParse.ElementsPattern[3]);

            Assert.AreEqual(1, lineParse.ElementsLog.Count);
            Assert.AreEqual(4, lineParse.ElementsLog[0].Elements.Count);
            Assert.AreEqual("2008-12-22 09:16:02,734", lineParse.ElementsLog[0].Elements[0]);
            Assert.AreEqual("[4904]", lineParse.ElementsLog[0].Elements[1]);
            Assert.AreEqual("INFO", lineParse.ElementsLog[0].Elements[2]);
            Assert.AreEqual(
            "Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components.",
            lineParse.ElementsLog[0].Elements[3]);

            lineParse.ParseLogLine(@"1234567890");
            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]|FAIL|Hsl3");
            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]|ERROR|Hsl5");
        }

        [Test]
        public void DoubleLineParsing()
        {
            LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Unsorted");
            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]|INFO|Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components.");
            lineParse.ParseLogLine(@" 2008-12-22 09:16:02,734|[4904]FAIL|Hsl3");

            Assert.AreEqual(
            "Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components. 2008-12-22 09:16:02,734|[4904]FAIL|Hsl3",
            lineParse.ElementsLog[0].Elements[3]);
        }

        [Test]
        public void ThreeElementsParsing()
        {
            LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Unsorted");
            lineParse.ParseLogLine(@"2008-12-22 09:16:02,734|[4904]|INFO|Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components.");
            lineParse.ParseLogLine(@"2008-12-22 09:16:05,577|[3152]|INFO|Hsl.UniversalHost.Core.ActiveComponent [Start Component SmsCenter] - Component 'SmsCenter' started.");
            lineParse.ParseLogLine(@"!T|ES|T!");
            lineParse.ParseLogLine(@"2008-12-22 09:37:59,977|[3368]|INFO|Hsl.Sms.Emi.Connection [Listen(1)] - Closing connection '1'.");

            Assert.AreEqual(3, lineParse.ElementsLog.Count);

            Assert.AreEqual(
            "Hsl.UniversalHost.|Core.Component|Manager [ComponentManager.Start] - Starting all components.",
            lineParse.ElementsLog[0].Elements[3]);

            Assert.AreEqual(
            "Hsl.UniversalHost.Core.ActiveComponent [Start Component SmsCenter] - Component 'SmsCenter' started.!T|ES|T!",
            lineParse.ElementsLog[1].Elements[3]);

            Assert.AreEqual(
            "Hsl.Sms.Emi.Connection [Listen(1)] - Closing connection '1'.",
            lineParse.ElementsLog[2].Elements[3]);
        }

        [Test]
        public void ParsingFromLogFile()
        {
            using (Stream fileStream = File.OpenRead(@"..\..\..\Data\Samples\TestLogParser.log"))
            {
                LogCollection lineParse = new LogCollection('|', "Time|ThreadId|Level|Unsorted");

                lineParse.ParseLogFile(fileStream);

                Assert.AreEqual(15, lineParse.ElementsLog.Count);
            }
        }

        [FixtureSetUp]
        public void FixtureSetup()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}