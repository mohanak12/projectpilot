using System;
using System.Collections.Generic;

namespace Accipio.Reporting
{
    public class TestCaseExecutionReport
    {
        /// <summary>
        /// Initializes a new instance of the TestCaseExecutionReport class.
        /// </summary>
        /// <param name="testCaseId">Test case id.</param>
        /// <param name="startTime">Start time of test case.</param>
        /// <param name="endTime">Test duration.</param>
        /// <param name="executionStatus">Status of test.</param>
        public TestCaseExecutionReport(
            string testCaseId, 
            DateTime startTime, 
            DateTime endTime, 
            TestCaseExecutionStatus executionStatus)
        {
            this.testCaseId = testCaseId;
            this.startTime = startTime;
            this.endTime = endTime;
            this.executionStatus = executionStatus;
        }

        /// <summary>
        /// Gets test case duration time.
        /// </summary>
        public TimeSpan CaseDuration
        {
            get { return endTime - startTime; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
        }

        /// <summary>
        /// Gets test case report executionStatus.
        /// </summary>
        public TestCaseExecutionStatus ExecutionStatus
        {
            get { return executionStatus; }
        }

        /// <summary>
        /// Gets test case start time.
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
        }

        /// <summary>
        /// Gets test case id.
        /// </summary>
        public string TestCaseId
        {
            get { return testCaseId; }
        }

        /// <summary>
        /// Gets list of user stories in single test case.
        /// </summary>
        public IList<string> UserStories
        {
            get { return userStories; }
        }

        /// <summary>
        /// Gets or sets report details.
        /// </summary>
        public string ReportDetails { get; set; }

        private readonly DateTime endTime;
        private readonly TestCaseExecutionStatus executionStatus;
        private readonly DateTime startTime;
        private readonly string testCaseId;
        private readonly List<string> userStories = new List<string>();
    }
}