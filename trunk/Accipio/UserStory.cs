using System.Collections.Generic;
using System.Globalization;

namespace Accipio
{
    public class UserStory
    {
        public UserStory(string userStoryName)
        {
            this.userStoryName = userStoryName;
        }

        public int PresentInTestCase { get; set; }

        public int SuccessfullyAccomplished { get; set; }

        /// <summary>
        /// Gets all test cases bind to one userstory.
        /// </summary>
        public IList<ReportCase> TestCases
        {
            get { return testCases; }
        }

        public string UserStoryName
        {
            get { return userStoryName; }
        }

        /// <summary>
        /// Adds test case to userstory.
        /// </summary>
        /// <param name="reportCase">Report case <see cref="ReportCase"/></param>
        public void AddReportCase(ReportCase reportCase)
        {
            testCases.Add(reportCase);
        }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture, 
                "{0} ({1}/{2})", 
                userStoryName, 
                SuccessfullyAccomplished,
                PresentInTestCase);
        }

        private readonly string userStoryName;
        private readonly List<ReportCase> testCases = new List<ReportCase>();
    }
}
