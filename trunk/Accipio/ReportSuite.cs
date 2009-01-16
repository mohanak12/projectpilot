using System.Collections.Generic;

namespace Accipio
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
        /// <param name="testCase">Test case.</param>
        public void AddTestCase(ReportCase testCase)
        {
            testCases.Add(testCase);
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
        public IList<ReportCase> TestCases
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

        private readonly List<ReportCase> testCases = new List<ReportCase>();
        private readonly string suiteId;
    }
}
