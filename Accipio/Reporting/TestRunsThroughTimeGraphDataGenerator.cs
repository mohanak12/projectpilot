using System.Collections.Generic;

namespace Accipio.Reporting
{
    public class TestRunsThroughTimeGraphDataGenerator : ITestReportGraphDataGenerator
    {
        public TestReportGraphData GenerateData(TestRunsDatabase testRunsDatabase)
        {
            TestReportGraphData graphData = new TestReportGraphData("test cases", "TestCasesHistoyGraph.png");
            graphData.AddSeries("successful", "#75FF47");
            graphData.AddSeries("failed", "#FF6B90");
            graphData.AddSeries("not implemented", "#FFFCA8");
                
            foreach (TestRun testRun in testRunsDatabase.TestRuns)
            {
                graphData.AddDataValue("successful", testRun.TestCasesSuccess);
                graphData.AddDataValue("failed", testRun.TestCasesFail);
                graphData.AddDataValue("not implemented", testRun.TestCasesNotImplemented);
            }

            return graphData;
        }
    }
}