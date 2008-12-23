using System;
using System.IO;
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
            Stream stream = File.OpenRead(@"..\..\..\Data\Samples\TestServer.log");
            LogCollection parseCollection = new LogCollection(@"date time threadId level other");
            parseCollection.ParseLogFile(stream);
        }
    }
}