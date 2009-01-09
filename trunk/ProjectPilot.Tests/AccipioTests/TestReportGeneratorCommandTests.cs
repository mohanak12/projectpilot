using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class TestReportGeneratorCommandTests
    {
        [Test, Pending("Not finished yet")]
        public void Transform()
        {
            string[] args = new string[]
                                {
                                    "transform",
                                    "..\\..\\..\\Data\\Samples\\TestResults-0.xml",
                                    "TestResults.xml"
                                };
            TestReportGeneratorCommand testReportGeneratorCommand = new TestReportGeneratorCommand(null);
            testReportGeneratorCommand.AccipioDirectory = "..\\..\\..\\Accipio.Console";
            testReportGeneratorCommand.ParseArguments(args);
            testReportGeneratorCommand.ProcessCommand();
        }
    }
}