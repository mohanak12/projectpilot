
using System.Collections;
using System.Globalization;

namespace Accipio
{
    public class UserStory
    {
        public UserStory(string userStoryName)
        {
            this.userStoryName = userStoryName;
        }

        public int PresenceInTestCase { get; set; }

        public int SuccessfullyAccomplished { get; set; }

        public string UserStoryName
        {
            get { return userStoryName; }
        }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture, 
                "{0} ({1}/{2})", 
                userStoryName, 
                SuccessfullyAccomplished,
                PresenceInTestCase);
        }

        private readonly string userStoryName;
    }
}
