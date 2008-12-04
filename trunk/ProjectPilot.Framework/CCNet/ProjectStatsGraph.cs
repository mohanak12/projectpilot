using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    /// <summary>
    /// Saves all data for single graph
    /// </summary>
    public class ProjectStatsGraph
    {
        /// <summary>
        /// Adds graph parameters to list
        /// </summary>
        /// <typeparam name="T">type of parameter</typeparam>
        /// <param name="parameterName">parameter name</param>
        /// <param name="seriesColor">line color, that will be shown on graph</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void AddParameter<T>(string parameterName, string seriesColor)
        {
            parameters.Add(new ProjectStatsGraphParameter(parameterName, typeof(T), seriesColor));
        }

        /// <summary>
        /// Gets list of graph parameters
        /// </summary>
        public IList<ProjectStatsGraphParameter> GraphParameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Gets or sets graph name
        /// </summary>
        public string GraphName { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether unsuccessful builds will be considered
        /// </summary>
        public bool IgnoreFailures { set; get; }

        /// <summary>
        /// Gets or sets labels on X-Axis
        /// </summary>
        public string XAxisTitle { set; get; }

        /// <summary>
        /// Gets or sets labels on Y-Axis
        /// </summary>
        public string YAxisTitle { set; get; }

        private readonly List<ProjectStatsGraphParameter> parameters = new List<ProjectStatsGraphParameter>();
    }
}
