using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using log4net;
using MbUnit.Framework;
using ProjectPilot.Extras.LogParser;
using ProjectPilot.Extras.LogParser.Log4NetPatterns;

namespace ProjectPilot.Tests.Extras
{
    [TestFixture]
    public class LogParserTest
    {
        [Test]
        public void SimpleTimestampPattern()
        {
            ParsedPatternLayout patternParse = new ParsedPatternLayout();

            patternParse.ParsePattern("%timestamp");
            Assert.AreEqual(1, patternParse.Elements.Count);

            //patternParse.Elements

            Assert.IsInstanceOfType(typeof(TimestampPatternLayoutElement), patternParse.Elements[0]);
        }

        [Test]
        public void SimpleTimestampPatternWithLiteral()
        {
            ParsedPatternLayout patternParse = new ParsedPatternLayout();

            patternParse.ParsePattern("[%timestamp");
            Assert.AreEqual(2, patternParse.Elements.Count);

            LiteralPatternLayoutElement element = (LiteralPatternLayoutElement) patternParse.Elements[0];
            Assert.AreEqual("[", element.LiteralText);

            Assert.IsInstanceOfType(typeof(LiteralPatternLayoutElement), patternParse.Elements[0]);
            Assert.IsInstanceOfType(typeof(TimestampPatternLayoutElement), patternParse.Elements[1]);
        }

        [Test]
        public void ComplexPatternWithSpaces()
        {
            ParsedPatternLayout patternParse = new ParsedPatternLayout();

            patternParse.ParsePattern("%timestamp [%thread]%level%logger%ndc - %message%newline");

            int i = 0;
            Assert.IsInstanceOfType(typeof(TimestampPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LiteralPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(ThreadPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LiteralPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LevelPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LoggerPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(NdcPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LiteralPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(MessagePatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(NewLinePatternLayoutElement), patternParse.Elements[i++]);
            
            Assert.AreEqual(10, patternParse.Elements.Count);

            LiteralPatternLayoutElement element = (LiteralPatternLayoutElement)patternParse.Elements[1];
            Assert.AreEqual(" [", element.LiteralText);

            element = (LiteralPatternLayoutElement)patternParse.Elements[3];
            Assert.AreEqual("]", element.LiteralText);

            element = (LiteralPatternLayoutElement)patternParse.Elements[7];
            Assert.AreEqual(" - ", element.LiteralText);
        }

        [Test]
        public void ComplexPatternWithoutSpaces()
        {
            ParsedPatternLayout patternParse = new ParsedPatternLayout();

            patternParse.ParsePattern("%timestamp[%thread]%level%logger%ndc-%message%newline");

            int i = 0;
            Assert.IsInstanceOfType(typeof(TimestampPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LiteralPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(ThreadPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LiteralPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LevelPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LoggerPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(NdcPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(LiteralPatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(MessagePatternLayoutElement), patternParse.Elements[i++]);
            Assert.IsInstanceOfType(typeof(NewLinePatternLayoutElement), patternParse.Elements[i++]);

            Assert.AreEqual(10, patternParse.Elements.Count);

            LiteralPatternLayoutElement element = (LiteralPatternLayoutElement)patternParse.Elements[1];
            Assert.AreEqual("[", element.LiteralText);

            element = (LiteralPatternLayoutElement)patternParse.Elements[3];
            Assert.AreEqual("]", element.LiteralText);

            element = (LiteralPatternLayoutElement)patternParse.Elements[7];
            Assert.AreEqual("-", element.LiteralText);
        }

        [Test, Pending]
        public void SimpleLineParsing()
        {
            //ParsedLayout lineParse = new ParsedLayout(Time ThreadId);
            //lineParse.Parse(@"2008-12-22 09:16:02,734|[4904]);
            //Assert(2, LineParse.element.count);
            //Assert(2, LineParse.elementValue.count);
            //Assert.AreEqual("Time", lineParse.element[0].value);
            //Assert.AreEqual("ThreadId", lineParse.element[1].value);
            //Assert.AreEqual("[4904]", lineParse.elementValue[1].value);
        }

        [Test, Pending]
        public void ComplexLineParsing()
        {
            //ParsedLayout lineParse = new ParsedLayout(Time ThreadId Level Unsorted);
            //lineParse.Parse(@"2008-12-22 09:16:02,734|[4904]|INFO|Hsl.UniversalHost.Core.ComponentManager [ComponentManager.Start] - Starting all components.");
            //Assert(4, LineParse.element.count);
            //Assert(4, LineParse.elementValue.count);
            //Assert.AreEqual("Time", lineParse.element[0].value);
            //Assert.AreEqual("ThreadId", lineParse.element[1].value);
            //Assert.AreEqual("[4904]", LineParse.elementValue[1].value);
            //Assert.AreEqual("INFO", LineParse.elementValue[2].value);
            //Assert.AreEqual("Level", lineParse.element[3].value);
            //Assert.AreEqual("Unsorted", lineParse.element[4].value);
        }

        [Test, Pending]
        public void DoubleLineParsing()
        {
            //ParsedLayout lineParse = new ParsedLayout(Time ThreadId Level Unsorted);
            //lineParse.Parse(
            //@"2008-12-22 09:16:02,734|[4904]|INFO|Hsl.UniversalHost.Core.ComponentManager [ComponentManager.Start] - Starting all components.\n Exception rethrown at [0]: at System.Runtime.Remoting.Proxies.RealProxy.HandleReturnMessage(IMessage reqMsg, IMessage retMsg)");
            //...
        }

        [FixtureSetUp]
        public void FixtureSetup()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}