#region

using Accipio;
using Accipio.Console;
using MbUnit.Framework;

#endregion

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class TestReportsTests
    {
        [Test]
        public void TestReportsData()
        {
            TestReports testReports = new TestReports();
            testReports.AddUserStoryName("story1");
            Assert.IsNotNull(testReports.UserStories);
            ReportData reportData = new ReportData { Version = "1" };
            testReports.Reports.Add("story1", reportData);
            Assert.IsNotNull(testReports.Reports);
        }

        [Test]
        public void GenerateUserStoryHistory()
        {
            IConsoleCommand consoleCommand = new UserStoryGeneratorCommand(null)
                .ParseArguments(new[] { "userstory", @"..\..\..\Data\Samples\TestReports", @"..\..\..\Data\Samples\UserStories" });
            consoleCommand.AccipioDirectory = string.Empty;
            // process command
            consoleCommand.ProcessCommand();
        }
    }
}