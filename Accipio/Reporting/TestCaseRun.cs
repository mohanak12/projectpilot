using System;
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

        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
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

        public IDictionary<string, string> UserStories
        {
            get { return userStories; }
        }

        public void AddUserStory (string userStoryId)
        {
            userStories.Add(userStoryId, null);
        }

        private TimeSpan duration;
        private TestExecutionStatus status;
        private string message;
        private string testCaseId;
        private Dictionary<string, string> userStories = new Dictionary<string, string>();
    }
}