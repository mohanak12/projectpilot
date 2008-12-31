using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using ProjectPilot.Extras.LogParser;
using ProjectPilot.Extras.LogParser.Log4NetPatterns;

namespace ProjectPilot.Tests.Extras
{
    [TestFixture]
    public class Log4NetParserTests
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
    }
}
