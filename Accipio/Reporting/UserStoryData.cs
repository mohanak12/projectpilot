using System.Collections;
using System.Collections.Generic;
using Accipio.Reporting;

namespace Accipio.Reporting
{
    public class UserStoryData
    {
        public IList<TestCaseExecutionReport> TestCases
        {
            get { return testCases; }
        }

        public void AddReportCase(TestCaseExecutionReport testCaseExecutionReport)
        {
            testCases.Add(testCaseExecutionReport);
        }

        private readonly List<TestCaseExecutionReport> testCases = new List<TestCaseExecutionReport>();
    }
}