using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsGraph
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void AddParameter(string graphName, IDictionary<string, List<double>> parametersList)
        {
            parameters.Add(new CCNetProjectStatisticsGraphParameter(graphName, parametersList));
        }

        private List<CCNetProjectStatisticsGraphParameter> parameters = new List<CCNetProjectStatisticsGraphParameter>();

        public IList<CCNetProjectStatisticsGraphParameter> GraphParameters
        {
            get { return parameters; }
        }
    }
}
