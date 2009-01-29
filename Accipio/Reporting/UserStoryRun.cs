using System.Collections.Generic;
using System.Linq;

namespace Accipio.Reporting
{
    public class UserStoryRun
    {
        public TestExecutionStatus Status
        {
            get
            {
                bool allSuccessful = true;

                foreach (TestCaseRun testCaseRun in testCasesRuns)
                {
                    if (testCaseRun.Status == TestExecutionStatus.NotImplemented)
                        return TestExecutionStatus.NotImplemented;
                    if (testCaseRun.Status == TestExecutionStatus.Failed)
                        allSuccessful = false;
                }

                return allSuccessful ? TestExecutionStatus.Successful : TestExecutionStatus.Failed;
            }
        }

        public int TestCasesTotal
        {
            get { return testCasesRuns.Count; }
        }

        public int TestCasesSuccess
        {
            get
            {
                return testCasesRuns.Count(n => n.Status == TestExecutionStatus.Successful);
            }
        }

        public int TestCasesFail
        {
            get
            {
                return testCasesRuns.Count(n => n.Status == TestExecutionStatus.Failed);
            }
        }

        public int TestCasesNotImplemented
        {
            get
            {
                return testCasesRuns.Count(n => n.Status == TestExecutionStatus.NotImplemented);
            }
        }

        private List<TestCaseRun> testCasesRuns = new List<TestCaseRun>();
    }
}