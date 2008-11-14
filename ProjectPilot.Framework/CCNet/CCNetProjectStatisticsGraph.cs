using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsGraph
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void AddParameter<T>(string parameterName)
        {
            parameters.Add(new CCNetProjectStatisticsGraphParameter(parameterName, typeof(T)));
        }

        private List<CCNetProjectStatisticsGraphParameter> parameters = new List<CCNetProjectStatisticsGraphParameter>();

        public IList<CCNetProjectStatisticsGraphParameter> GraphParameters
        {
            get { return parameters; }
        }
    }
}
