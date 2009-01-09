using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Accipio
{
    public class ReportCase
    {
        /// <summary>
        /// Initializes a new instance of the ReportCase class.
        /// </summary>
        /// <param name="caseId">Test case id.</param>
        /// <param name="startTime">Start time of test case.</param>
        /// <param name="duration">Test duration.</param>
        /// <param name="status">Status of test.</param>
        public ReportCase(string caseId, string startTime, string duration, ReportCaseStatus status)
        {
            this.caseId = caseId;
            this.startTime = startTime;
            this.duration = duration;
            this.status = status;
        }

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
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string caseId;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string startTime;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string duration;
        private ReportCaseStatus status;
    }
}
