using System.Web;

namespace ProjectPilot.Portal.Models
{
    public class DefaultProjectPilotWebContext : IProjectPilotWebContext
    {
        public DefaultProjectPilotWebContext(HttpSessionStateBase session)
        {
            this.session = session;
        }

        public string CurrentModuleName
        {
            get { return (string) session["CurrentModuleName"]; }
            set { session["CurrentModuleName"] = value; }
        }

        public string CurrentProjectName
        {
            get { return (string)session["CurrentProjectName"]; }
            set { session["CurrentProjectName"] = value; }
        }

        private readonly HttpSessionStateBase session;
    }
}