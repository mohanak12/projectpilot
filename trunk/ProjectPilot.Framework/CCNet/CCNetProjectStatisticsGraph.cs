using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsGraph
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void AddParameter<T>(string graphColor, IList<double> parameterList,
            string parameterName)
        {
            parameters.Add(new CCNetProjectStatisticsGraphParameter(graphColor, parameterList, parameterName, typeof(T)));
        }

        public IList<CCNetProjectStatisticsGraphParameter> GraphParameters
        {
            get { return parameters; }
        }

        public string GraphName { set; get; }

        private List<CCNetProjectStatisticsGraphParameter> parameters = new List<CCNetProjectStatisticsGraphParameter>();
    }
}
