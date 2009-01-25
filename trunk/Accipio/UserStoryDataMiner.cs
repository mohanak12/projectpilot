using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Accipio
{
    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public class UserStoryDataMiner
    {
        public static IList<UserStory> GetUserStoryDetails(ReportData reportData)
        {
            List<UserStory> userStories = new List<UserStory>();

            foreach (ReportSuite reportSuite in reportData.TestSuites)
            {
                foreach (ReportCase reportCase in reportSuite.TestCases)
                {
                    foreach (string userStory in reportCase.UserStories)
                    {
                        UserStory story = userStories.Find(temp => temp.UserStoryName == userStory);

                        if (story == null)
                        {
                            story = new UserStory(userStory);

                            userStories.Add(story);
                        }

                        if (reportCase.Status == ReportCaseStatus.Passed)
                            story.SuccessfullyAccomplished++;

                        story.PresentInTestCase++;

                        story.AddReportCase(reportCase);
                    }
                }
            }

            return userStories;
        }
    }
}
