using System;
using System.Collections.Generic;
using System.Globalization;

namespace ProjectPilot.Framework.CCNet
{
    /// <summary>
    /// Saves statistic data for each build
    /// </summary>
    public class ProjectStatsBuildEntry
    {
        /// <summary>
        /// Initializes a new instance of the ProjectStatsBuildEntry class.
        /// </summary>
        /// <param name="buildId">Id of build</param>
        public ProjectStatsBuildEntry(int buildId)
        {
            this.buildId = buildId;
        }

        /// <summary>
        /// Gets id of build
        /// </summary>
        public int BuildId
        {
            get { return buildId; }
        }

        /// <summary>
        /// Gets or sets lable of build
        /// </summary>
        public string BuildLabel { get; set; }

        /// <summary>
        /// Gets project build start time
        /// </summary>
        public DateTime BuildStartTime
        {
            get
            {
                return DateTime.Parse(parameters["StartTime"], CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets list of paramaters for specified build
        /// </summary>
        public IDictionary<string, string> Parameters
        {
            get { return parameters; }
        }

        private int buildId;
        private Dictionary<string, string> parameters = new Dictionary<string, string>();
    }
}