using Accipio;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class TestReportsTests
    {
        [Test]
        public void TestReportsData()
        {
            TestReports testReports = new TestReports();
            testReports.AddUserStory("story1");
            Assert.IsNotNull(testReports.UserStories);
            ReportData reportData = new ReportData();
            reportData.Version = "1";
            testReports.Reports.Add("story1", reportData);
            Assert.IsNotNull(testReports.Reports);
        }
    }
}