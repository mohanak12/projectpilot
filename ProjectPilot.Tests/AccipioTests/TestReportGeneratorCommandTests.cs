using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class TestReportGeneratorCommandTests
    {
        [Test]
        public void TransformGallioTestResults()
        {
            string[] args = new string[]
                                {
                                    "transform",
                                    "..\\..\\..\\Data\\Samples\\AcceptanceTestResults.xml",
                                    "TestResults.xml"
                                };
            TestReportGeneratorCommand testReportGeneratorCommand =
                new TestReportGeneratorCommand(null)
                    {
                        AccipioDirectory = string.Empty
                    };
            testReportGeneratorCommand.ParseArguments(args);
            testReportGeneratorCommand.ProcessCommand();
        }
    }
}