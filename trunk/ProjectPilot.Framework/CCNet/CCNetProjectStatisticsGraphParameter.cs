using System;
using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    /// <summary>
    /// CCNetProjectStatisticsGraphParameter contains all parameters for one graph.
    /// </summary>
    public class CCNetProjectStatisticsGraphParameter
    {
        public CCNetProjectStatisticsGraphParameter(string graphColor, string parameterName, Type parameterType)
        {
            this.graphColor = graphColor;
            this.parameterName = parameterName;
            this.parameterType = parameterType;
        }
        
        public string GraphColor
        {
            get { return graphColor; }
        }

        public IList<double> ParameterList
        {
            get { return parameterList; }
        }

        public string ParameterName
        {
            get { return parameterName; }
        }

        public Type ParameterType
        {
            get { return parameterType; }
        }

        private string graphColor;
        private IList<double> parameterList = new List<double>();
        private string parameterName;
        private Type parameterType;
    }
}