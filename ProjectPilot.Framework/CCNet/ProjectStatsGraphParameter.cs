using System;

namespace ProjectPilot.Framework.CCNet
{
    /// <summary>
    /// Saves ccnet statistics parameter name, parameter type and color
    /// </summary>
    public class ProjectStatsGraphParameter
    {
        public ProjectStatsGraphParameter(string parameterName, Type parameterType, string seriesColor)
        {
            this.seriesColor = seriesColor;
            this.parameterName = parameterName;
            this.parameterType = parameterType;
        }
        
        /// <summary>
        /// Gets color of graph
        /// </summary>
        public string SeriesColor
        {
            get { return seriesColor; }
        }

        /// <summary>
        /// Gets parameter name
        /// </summary>
        public string ParameterName
        {
            get { return parameterName; }
        }

        /// <summary>
        /// Gets parameter type
        /// </summary>
        public Type ParameterType
        {
            get { return parameterType; }
        }

        private readonly string seriesColor;
        private readonly string parameterName;
        private readonly Type parameterType;
    }
}