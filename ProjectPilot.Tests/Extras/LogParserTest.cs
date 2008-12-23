using System.IO;
using MbUnit.Framework;
using ProjectPilot.Extras.LogParser;

namespace ProjectPilot.Tests.Extras
{
    [TestFixture]
    public class LogParserTest
    {
        [Test]
        public void TestLogParser()
        {
            Stream stream = File.OpenRead(@"..\..\..\Data\Samples\TestServer.log");
            LogCollection parseCollection = new LogCollection();
            parseCollection.ParseLogFile(stream);
        }
    }
}