using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

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
    }
}
