using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsBuildEntry
    {
        #region Private members

        private string buildLabel;
        private string buildStatus;
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        #endregion

        #region Public properties

        public string BuildLabel
        {
            get { return buildLabel; }
            set { buildLabel = value; }
        }

        public string BuildStatus
        {
            get { return buildStatus; }
            set { buildStatus = value; }
        }

        public IDictionary<string, string> Parameters
        {
            get { return parameters; }
        }

        #endregion
    }
}