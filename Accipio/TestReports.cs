using System;
using System.Collections.Generic;
using System.Globalization;

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
        /// Gets the ollection of userstories names.
        /// </summary>
        public IList<string> UserStories
        {
            get { return userStories; }
        }

        /// <summary>
        /// Adds new item to collection.
        /// </summary>
        /// <param name="fileName">Name of the key</param>
        /// <param name="reportData">Data from report.</param>
        public void AddReportData(string fileName, ReportData reportData)
        {
            if (reports.ContainsKey(fileName))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Key '{0}' allredy added!", fileName));
            }

            reports.Add(fileName, reportData);
        }

        /// <summary>
        /// Add new userstory to collection.
        /// </summary>
        /// <param name="userStoryName">Name of the userStory</param>
        public void AddUserStoryName(string userStoryName)
        {
            if (!userStories.Contains(userStoryName))
            {
                userStories.Add(userStoryName);
            }
        }

        private readonly Dictionary<string, ReportData> reports = new Dictionary<string, ReportData>();
        private readonly List<string> userStories = new List<string>();
    }
}