using System;
using System.Collections.Generic;

namespace Accipio
{
    public class ReportCase
    {
        /// <summary>
        /// Initializes a new instance of the ReportCase class.
        /// </summary>
        /// <param name="caseId">Test case id.</param>
        /// <param name="caseStartTime">Start time of test case.</param>
        /// <param name="caseDuration">Test duration.</param>
        /// <param name="status">Status of test.</param>
        public ReportCase(string caseId, DateTime caseStartTime, string caseDuration, ReportCaseStatus status)
        {
            this.caseId = caseId;
            this.caseStartTime = caseStartTime;
            this.caseDuration = caseDuration;
            this.status = status;
        }

        /// <summary>
        /// Gets test case duration time.
        /// </summary>
        public string CaseDuration
        {
            get { return caseDuration; }
        }

        /// <summary>
        /// Gets test case id.
        /// </summary>
        public string CaseId
        {
            get { return caseId; }
        }

        /// <summary>
        /// Gets test case start time.
        /// </summary>
        public DateTime CaseStartTime
        {
            get { return caseStartTime; }
        }

        /// <summary>
        /// Gets or sets report details.
        /// </summary>
        public string ReportDetails { get; set; }

        /// <summary>
        /// Gets list of user stories in single test case.
        /// </summary>
        public IList<string> UserStories
        {
            get { return userStories; }
        }

        /// <summary>
        /// Gets test case report status.
        /// </summary>
        public ReportCaseStatus Status
        {
            get { return status; }
        }

        private readonly List<string> userStories = new List<string>();
        private readonly string caseId;
        private readonly DateTime caseStartTime;
        private readonly string caseDuration;
        private readonly ReportCaseStatus status;
    }
}
