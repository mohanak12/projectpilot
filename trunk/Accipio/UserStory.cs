using System.Collections.Generic;
using System.Globalization;

namespace Accipio
{
    public class UserStory
    {
        /// <summary>
        /// Initializes a new instance of the UserStory class.
        /// </summary>
        /// <param name="userStoryName">Define user story name.</param>
        public UserStory(string userStoryName)
        {
            this.userStoryName = userStoryName;
        }

        /// <summary>
        /// Gets or sets number of test cases where user story is presented.
        /// </summary>
        public int PresentInTestCase { get; set; }

        /// <summary>
        /// Gets or sets number of successfuly accomplished test case that contains user story.
        /// </summary>
        public int SuccessfullyAccomplished { get; set; }

        /// <summary>
        /// Gets or sets number of failed tests.
        /// </summary>
        public int Failed { get; set; }

        /// <summary>
        /// Gets or sets number of skipped tests.
        /// </summary>
        public int Skipped { get; set; }

        /// <summary>
        /// Gets all test cases bind to one userstory.
        /// </summary>
        public IList<ReportCase> TestCases
        {
            get { return testCases; }
        }

        /// <summary>
        /// Gets user story name.
        /// </summary>
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
