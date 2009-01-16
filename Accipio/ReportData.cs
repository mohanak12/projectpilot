using System;
using System.Collections.Generic;

namespace Accipio
{
    public class ReportData
    {
        /// <summary>
        /// Gets or sets duration of tests.
        /// </summary>
        public string Duration
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets start time when tests are started up.
        /// </summary>
        public DateTime StartTime
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets version of test report. 
        /// </summary>
        public string Version
        {
            get; set;
        }

        /// <summary>
        /// Gets list of test suites.
        /// </summary>
        public IList<ReportSuite> TestSuites
        {
            get { return testSuites; }
        }

        private readonly List<ReportSuite> testSuites = new List<ReportSuite>();
    }
}
