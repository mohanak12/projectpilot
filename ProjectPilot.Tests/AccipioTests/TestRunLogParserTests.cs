using Accipio.Reporting;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class TestRunLogParserTests
    {
        [Test]
        public void Test()
        {
            string reportDataFileName = @"..\..\..\Data\Samples\TestResults.xml";

            TestRunLogParser parser = new TestRunLogParser(reportDataFileName);

            TestRun testRun = parser.Parse();

            Assert.IsNotNull(testRun);
        }
    }
}
