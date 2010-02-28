using System;
using System.Collections.Generic;
using System.Text;

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

        public IList<Attachment> Attachments
        {
            get { return attachments; }
        }

        public void AddUserStory(string userStoryId)
        {
            userStories.Add(userStoryId, null);
        }

        public void AddAttachment(Attachment attachment)
        {
            attachments.Add(attachment);
        }

        private TimeSpan duration;
        private TestExecutionStatus status;
        private string message;
        private string testCaseId;
        private Dictionary<string, string> userStories = new Dictionary<string, string>();
        private List<Attachment> attachments = new List<Attachment>();
    }
}