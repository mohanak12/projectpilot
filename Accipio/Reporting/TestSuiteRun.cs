using System;
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

        public IDictionary<string, TestCaseRun> TestCasesRuns
        {
            get { return testCasesRuns; }
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

        public IEnumerable<TestCaseRun> ListTestCasesRuns()
        {
            List<TestCaseRun> sorted = new List<TestCaseRun>(testCasesRuns.Values);
            sorted.Sort((a, b) => string.Compare(a.TestCaseId, b.TestCaseId, StringComparison.OrdinalIgnoreCase));
            return sorted;
        }

        private Dictionary<string, TestCaseRun> testCasesRuns = new Dictionary<string, TestCaseRun>();
        private string testSuiteId;
    }
}