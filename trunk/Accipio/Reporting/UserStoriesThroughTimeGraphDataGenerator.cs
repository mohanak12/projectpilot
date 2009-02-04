namespace Accipio.Reporting
{
    public class UserStoriesThroughTimeGraphDataGenerator : ITestReportGraphDataGenerator
    {
        public TestReportGraphData GenerateData(TestRunsDatabase testRunsDatabase)
        {
            TestReportGraphData data = new TestReportGraphData("user stories", "UserStoriesHistoyGraph.png");
            data.AddSeries("success", "failed", "pending");

            foreach (TestRun testRun in testRunsDatabase.TestRuns)
            {
                data.AddDataValue("success", testRun.UserStoriesSuccess);
                data.AddDataValue("failed", testRun.UserStoriesFail);
                data.AddDataValue("pending", testRun.UserStoriesNotImplemented);
            }

            return data;
        }
    }
}