using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    public class ProjectStatsGraph
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void AddParameter<T>(string parameterName, string seriesColor)
        {
            parameters.Add(new ProjectStatsGraphParameter(parameterName, typeof(T), seriesColor));
        }

        public IList<ProjectStatsGraphParameter> GraphParameters
        {
            get { return parameters; }
        }

        public string GraphName { set; get; }

        public string XAxisTitle { set; get; }

        public string YAxisTitle { set; get; }

        private readonly List<ProjectStatsGraphParameter> parameters = new List<ProjectStatsGraphParameter>();
    }
}
