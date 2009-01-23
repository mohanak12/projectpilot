using System;
using System.Collections.Generic;

namespace Accipio
{
    /// <summary>
    /// Holds collection of reports from all parsed files
    /// </summary>
    public class TestReports
    {
        /// <summary>
        /// Gets reports collection.
        /// </summary>
        public IDictionary<string, ReportData> Reports
        {
            get { return reports; }
        }

        /// <summary>
        /// Gets the collection of UserStory names.
        /// </summary>
        public IList<string> UserStories
        {
            get { return userStories; }
        }

        /// <summary>
        /// Adds new item to collection.
        /// </summary>
        /// <param name="userStory">Name of userStory</param>
        /// <param name="reportData">Data from report.</param>
        public void AddReportData(string userStory, ReportData reportData)
        {
            if (reports.ContainsKey(userStory))
            {
                throw new ArgumentException("UserStory allredy added!");
            }

            reports.Add(userStory, reportData);
        }

        /// <summary>
        /// Adds new item to collection.
        /// </summary>
        /// <param name="userStory">Name of userStory</param>
        public void AddUserStory(string userStory)
        {
            if (!UserStories.Contains(userStory))
            {
                UserStories.Add(userStory);
            }
            else
                throw new ArgumentException("UserStory allredy added!");
        }

        private readonly Dictionary<string, ReportData> reports = new Dictionary<string, ReportData>();
        private readonly List<string> userStories = new List<string>();
    }
}