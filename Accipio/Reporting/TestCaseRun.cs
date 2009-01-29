using System.Collections.Generic;

namespace Accipio.Reporting
{
    public class TestCaseRun
    {
        public TestCaseRun(string testCaseId, TestExecutionStatus status)
        {
            this.testCaseId = testCaseId;
            this.status = status;
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public TestExecutionStatus Status
        {
            get { return status; }
        }

        public string TestCaseId
        {
            get { return testCaseId; }
        }

        public void AddUserStory (string userStoryId)
        {
            userStories.Add(userStoryId, null);
        }

        private string testCaseId;
        private TestExecutionStatus status;
        private string message;
        private Dictionary<string, string> userStories = new Dictionary<string, string>();
    }
}