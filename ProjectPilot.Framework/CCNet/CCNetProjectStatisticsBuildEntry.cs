using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsBuildEntry
    {
        #region Private members

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string buildLabel;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string buildStatus;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
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

        public Dictionary<string, string> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        #endregion
    }
}