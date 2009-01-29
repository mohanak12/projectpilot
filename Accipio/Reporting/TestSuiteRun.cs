using System.Collections.Generic;
using System.Linq;

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

        public int TestCasesTotal
        {
            get { return testCasesRuns.Count; }
        }

        public int TestCasesSuccess
        {
            get
            {
                return testCasesRuns.Count(n => n.Value.Status == TestExecutionStatus.Successful);
            }
        }

        public int TestCasesFail
        {
            get
            {
                return testCasesRuns.Count(n => n.Value.Status == TestExecutionStatus.Failed);
            }
        }

        public int TestCasesNotImplemented
        {
            get
            {
                return testCasesRuns.Count(n => n.Value.Status == TestExecutionStatus.NotImplemented);
            }
        }

        public void AddTestCaseRun(TestCaseRun testCaseRun)
        {
            testCasesRuns.Add(testCaseRun.TestCaseId, testCaseRun);
        }

        private Dictionary<string, TestCaseRun> testCasesRuns = new Dictionary<string, TestCaseRun>();
        private string testSuiteId;
    }
}