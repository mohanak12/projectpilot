#region

using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Accipio.Console
{
    public class UserStoryGeneratorCommand : IConsoleCommand
    {
        public UserStoryGeneratorCommand(IConsoleCommand nextCommandInChain)
        {
            this.nextCommandInChain = nextCommandInChain;
        }

        /// <summary>
        /// Gets or sets the accipio output directory.
        /// </summary>
        /// <value>The accipio output directory.</value>
        public string AccipioDirectory { get; set; }

        /// <summary>
        /// Gets or sets the location with all transformed report files.
        /// </summary>
        public string FolderWithReportFiles { get; set; }

        /// <summary>
        /// Gets or sets the location where UserStories will be saved.
        /// </summary>
        public string FolderWithGeneratedUserStories { get; set; }

        /// <summary>
        /// Returns the first <see cref="IConsoleCommand"/> in the command chain 
        /// which can understand the provided command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>The first <see cref="IConsoleCommand"/> which can understand the provided command-line arguments
        /// or <c>null</c> if none of the console commands can understand them.</returns>
        public IConsoleCommand ParseArguments(string[] args)
        {
            if (args == null)
            {
                return null;
            }

            if (args.Length < 1
                || 0 != String.Compare(args[0], "userstory", StringComparison.OrdinalIgnoreCase))
            {
                if (nextCommandInChain != null)
                {
                    return nextCommandInChain.ParseArguments(args);
                }

                return null;
            }

            if (args.Length < 2)
            {
                throw new ArgumentException("Missing location of test reports files.");
            }
            // set acceptance test report file name
            FolderWithReportFiles = args[1];

            if (args.Length < 3)
            {
                throw new ArgumentException("Missing location where UserStories should be generated.");
            }

            FolderWithGeneratedUserStories = args[2];

            return this;
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        public void ProcessCommand()
        {
            TestReports testReports = new TestReports();
            ParseAllReports(testReports);

            foreach (string userStory in testReports.UserStories)
            {
                UserStoryData testCases = GetTestCasesForUserStory(testReports, userStory);

                using (ICodeWriter writer = new FileCodeWriter(Path.Combine(FolderWithGeneratedUserStories, userStory + ".htm")))
                {
                    UserStoryGenerator storyGenerator = new UserStoryGenerator(writer);
                    storyGenerator.Generate(testCases);
                }
            }
        }

        /// <summary>
        /// Collect all test cases thatare asociated with one user story.
        /// </summary>
        /// <param name="testReports">Colelction of test reports <see cref="TestReports"/></param>
        /// <param name="userStory">Nameof the user story</param>
        /// <returns>List of test cases</returns>
        private static UserStoryData GetTestCasesForUserStory(TestReports testReports, string userStory)
        {
            UserStoryData testCases = new UserStoryData();
            foreach (KeyValuePair<string, ReportData> keyValuePair in testReports.Reports)
            {
                IList<UserStory> userStories = UserStoryDataMiner.GetUserStoryDetails(keyValuePair.Value);
                foreach (UserStory story in userStories)
                {
                    if (story.UserStoryName == userStory)
                    {
                        foreach (ReportCase reportCase in story.TestCases)
                        {
                            testCases.AddReportCase(reportCase);
                        }
                    }
                }
            }

            return testCases;
        }

        /// <summary>
        /// Parsesall available reports.
        /// </summary>
        /// <param name="testReports">Colelction of test reports <see cref="TestReports"/></param>
        private void ParseAllReports(TestReports testReports)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(FolderWithReportFiles);
            FileInfo[] reportFiles = directoryInfo.GetFiles("*.xml");
            foreach (FileInfo fileInfo in reportFiles)
            {
                ReportData reportData;
                using (ReportDataParser parser = new ReportDataParser(fileInfo.FullName))
                {
                    reportData = parser.Parse();
                }

                testReports.Reports.Add(fileInfo.Name, reportData);
                IList<UserStory> userStories = UserStoryDataMiner.GetUserStoryDetails(reportData);
                foreach (UserStory userStory in userStories)
                {
                    testReports.AddUserStoryName(userStory.UserStoryName);
                }
            }
        }

        private readonly IConsoleCommand nextCommandInChain;
    }
}