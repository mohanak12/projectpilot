using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Accipio.Reporting;

namespace Accipio.Reporting
{
    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public class UserStoryDataMiner
    {
        /// <summary>
        /// Mining all user stories and its details from object <see cref="ReportData"/>.
        /// For each user story data miner gets following information:
        /// number of test cases where user story is presented,
        /// number of test cases where user story is successfully accomplished,
        /// number of test cases where user story failed or skipped
        /// </summary>
        /// <param name="reportData">Report data.</param>
        /// <returns>List of user stories with details.</returns>
        public static IList<UserStory> GetUserStoryDetails(ReportData reportData)
        {
            List<UserStory> userStories = new List<UserStory>();

            foreach (ReportSuite reportSuite in reportData.TestSuites)
            {
                foreach (TestCaseExecutionReport reportCase in reportSuite.TestCases)
                {
                    foreach (string userStory in reportCase.UserStories)
                    {
                        string tempStory = userStory;
                        UserStory story = userStories.Find(temp => temp.UserStoryId == tempStory);

                        if (story == null)
                        {
                            story = new UserStory(userStory);
                            // add user story to list
                            userStories.Add(story);
                        }

                        if (reportCase.ExecutionStatus == TestCaseExecutionStatus.Passed)
                            story.SuccessfullyAccomplished++;

                        if (reportCase.ExecutionStatus == TestCaseExecutionStatus.Failed)
                            story.Failed++;

                        if (reportCase.ExecutionStatus == TestCaseExecutionStatus.Skipped)
                            story.Skipped++;

                        story.PresentInTestCase++;

                        story.AddReportCase(reportCase);
                    }
                }
            }

            return userStories;
        }
    }
}