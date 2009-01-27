using System.Collections.Generic;
using Accipio.Reporting;

namespace Accipio.Reporting
{
    public class ReportSuite
    {
        /// <summary>
        /// Initializes a new instance of the ReportSuite class.
        /// </summary>
        /// <param name="suiteId">Id of test suite.</param>
        public ReportSuite(string suiteId)
        {
            this.suiteId = suiteId;
        }

        /// <summary>
        /// Add new test case to test suite.
        /// </summary>
        /// <param name="testCaseExecutionReport">Test case.</param>
        public void AddTestCase(TestCaseExecutionReport testCaseExecutionReport)
        {
            testCases.Add(testCaseExecutionReport);
        }

        /// <summary>
        /// Gets id of test suite.
        /// </summary>
        public string SuiteId
        {
            get { return suiteId; }
        }

        /// <summary>
        /// Gets list of test cases.
        /// </summary>
        public IList<TestCaseExecutionReport> TestCases
        {
            get { return testCases; }
        }

        /// <summary>
        /// Gets or sets number of success test cases.
        /// </summary>
        public int PassedTests { get; set; }

        /// <summary>
        /// Gets or sets number of success test cases.
        /// </summary>
        public int FailedTests { get; set; }

        /// <summary>
        /// Gets or sets number of success test cases.
        /// </summary>
        public int SkippedTests { get; set; }

        private readonly List<TestCaseExecutionReport> testCases = new List<TestCaseExecutionReport>();
        private readonly string suiteId;
    }
}