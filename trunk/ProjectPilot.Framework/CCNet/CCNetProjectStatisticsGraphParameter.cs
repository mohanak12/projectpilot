using System;

namespace ProjectPilot.Framework.CCNet
{
    public class CCNetProjectStatisticsGraphParameter
    {
        public CCNetProjectStatisticsGraphParameter (string parameterName, Type parameterType)
        {
            this.parameterName = parameterName;
            this.parameterType = parameterType;
        }

        public string ParameterName
        {
            get { return parameterName; }
        }

        public Type ParameterType
        {
            get { return parameterType; }
        }

        private string parameterName;
        private Type parameterType;
    }
}