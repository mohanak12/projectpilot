using System;

namespace ProjectPilot.Framework.CCNet
{
    /// <summary>
    /// Class ProjectStatsGraphParameter contains all parameters for one graph.
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

        public string ParameterName
        {
            get { return parameterName; }
        }

        public Type ParameterType
        {
            get { return parameterType; }
        }

        private readonly string seriesColor;
        private readonly string parameterName;
        private readonly Type parameterType;
    }
}