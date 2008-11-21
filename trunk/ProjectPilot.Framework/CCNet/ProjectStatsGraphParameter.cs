using System;
using System.Collections.Generic;

namespace ProjectPilot.Framework.CCNet
{
    /// <summary>
    /// ProjectStatsGraphParameter contains all parameters for one graph.
    /// </summary>
    public class ProjectStatsGraphParameter
    {
        public ProjectStatsGraphParameter(string parameterName, Type parameterType, string seriesColor)
        {
            this.seriesColor = seriesColor;
            this.parameterName = parameterName;
            this.parameterType = parameterType;
        }
        
        public string SeriesColor
        {
            get { return seriesColor; }
        }

        public IList<double> ValueList
        {
            get { return valueList; }
        }

        public string ParameterName
        {
            get { return parameterName; }
        }

        public Type ParameterType
        {
            get { return parameterType; }
        }

        private string seriesColor;
        private IList<double> valueList = new List<double>();
        private string parameterName;
        private Type parameterType;
    }
}