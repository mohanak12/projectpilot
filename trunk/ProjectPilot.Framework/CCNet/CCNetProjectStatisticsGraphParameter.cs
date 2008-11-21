using System;
using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsGraphParameter
    {
        public CCNetProjectStatisticsGraphParameter(string graphColor, IList<double> parameterList, 
            string parameterName, Type parameterType)
        {
            this.graphColor = graphColor;
            this.parameterList = parameterList;
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
        private IList<double> parameterList;
        private string parameterName;
        private Type parameterType;
    }
}