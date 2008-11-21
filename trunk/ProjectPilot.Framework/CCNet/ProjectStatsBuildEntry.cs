using System;
using System.Collections.Generic;
using System.Globalization;

namespace ProjectPilot.Framework.CCNet
{
    public class ProjectStatsBuildEntry
    {
        public ProjectStatsBuildEntry(int buildId)
        {
            this.buildId = buildId;
        }

        #region Public properties

        public int BuildId
        {
            get { return buildId; }
        }

        public string BuildLabel { get; set; }

        public DateTime BuildStartTime
        {
            get
            {
                return DateTime.Parse(parameters["StartTime"], CultureInfo.InvariantCulture);
            }
        }

        public IDictionary<string, string> Parameters
        {
            get { return parameters; }
        }

        #endregion

        #region Private members

        private int buildId;
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        #endregion
    }
}