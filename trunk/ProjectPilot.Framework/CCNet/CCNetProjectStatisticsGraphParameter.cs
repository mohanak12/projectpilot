using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsGraphParameter
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public CCNetProjectStatisticsGraphParameter(string graphName, IDictionary<string, List<double>> parametersList)
        {
            this.graphName = graphName;
            this.parametersList = parametersList;
        }

        public string GraphName
        {
            get { return graphName; }
        }


        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IDictionary<string, List<double>> ParametersList
        {
            get { return parametersList; }
        }

        private string graphName;
        private IDictionary<string, List<double>> parametersList;
    }
}