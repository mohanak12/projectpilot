using System.Collections.Generic;

namespace Accipio.Reporting
{
    public class TestRunsThroughTimeGraphDataGenerator : ITestReportGraphDataGenerator
    {
        public TestReportGraphData GenerateData(TestRunsDatabase testRunsDatabase)
        {
            TestReportGraphData data = new TestReportGraphData("test cases", "TestCasesHistoyGraph.png");
            data.AddSeries("success", "failed", "pending");

            foreach (TestRun testRun in testRunsDatabase.TestRuns)
            {
                data.AddDataValue("success", testRun.TestCasesSuccess);
                data.AddDataValue("failed", testRun.TestCasesFail);
                data.AddDataValue("pending", testRun.TestCasesNotImplemented);
            }

            return data;
        }
    }
}