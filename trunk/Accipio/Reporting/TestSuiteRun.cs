using System.Collections.Generic;

namespace Accipio.Reporting
{
    public class TestSuiteRun
    {
        public TestSuiteRun(string testSuiteId)
        {
            this.testSuiteId = testSuiteId;
        }

        public string TestSuiteId
        {
            get { return testSuiteId; }
        }

        public void AddTestCaseRun (TestCaseRun testCaseRun)
        {
            testCasesRuns.Add(testCaseRun.TestCaseId, testCaseRun);
        }

        private Dictionary<string, TestCaseRun> testCasesRuns = new Dictionary<string, TestCaseRun>();
        private string testSuiteId;
    }
}