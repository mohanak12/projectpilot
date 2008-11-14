using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsBuildEntry
    {
        #region Public properties

        public string BuildLabel { get; set; }

        public IDictionary<string, string> Parameters
        {
            get { return parameters; }
        }

        #endregion

        #region Private members

        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        #endregion
    }
}