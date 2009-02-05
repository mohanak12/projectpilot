namespace Accipio.Reporting
{
    public class UserStoriesThroughTimeGraphDataGenerator : ITestReportGraphDataGenerator
    {
        public TestReportGraphData GenerateData(TestRunsDatabase testRunsDatabase)
        {
            TestReportGraphData graphData = new TestReportGraphData("user stories", "UserStoriesHistoyGraph.png");
            graphData.AddSeries("successful", "#75FF47");
            graphData.AddSeries("failed", "#FF6B90");
            graphData.AddSeries("not implemented", "#FFFCA8");

            foreach (TestRun testRun in testRunsDatabase.TestRuns)
            {
                graphData.AddDataValue("successful", testRun.UserStoriesSuccess);
                graphData.AddDataValue("failed", testRun.UserStoriesFail);
                graphData.AddDataValue("not implemented", testRun.UserStoriesNotImplemented);
            }

            return graphData;
        }
    }
}