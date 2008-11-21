using System;
using System.Collections.Generic;
using System.Globalization;

namespace ProjectPilot.Framework.CCNet
{
    public class ProjectStatsBuildEntry
    {
        #region Public properties

        public string BuildLabel { get; set; }

        public IDictionary<string, string> Parameters
        {
            get { return parameters; }
        }

        public DateTime BuildStartTime
        {
            get
            {
                return DateTime.Parse(parameters["StartTime"], CultureInfo.InvariantCulture);
            }
        }

        #endregion

        #region Private members

        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        #endregion
    }
}