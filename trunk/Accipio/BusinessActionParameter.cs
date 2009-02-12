using System;

namespace Accipio
{
    /// <summary>
    /// Saves business action parameter name and parameter type
    /// </summary>
    public class BusinessActionParameter
    {
        /// <summary>
        /// Initializes a new instance of the BusinessActionParameter class.
        /// </summary>
        /// <param name="parameterName">Name of parameter</param>
        /// <param name="parameterType">Type of parameter</param>
        /// <param name="parameterXsdType">The XML schema type of the parameter</param>
        /// <param name="parameterOrder">Order number of the parameter in the parameter list.</param>
        public BusinessActionParameter(
            string parameterName, 
            Type parameterType, 
            string parameterXsdType,
            int parameterOrder)
        {
            this.parameterName = parameterName;
            this.parameterType = parameterType;
            this.parameterXsdType = parameterXsdType;
            this.parameterOrder = parameterOrder;
        }

        /// <summary>
        /// Gets parameter name
        /// </summary>
        public string ParameterName
        {
            get { return parameterName; }
        }

        public int ParameterOrder
        {
            get { return parameterOrder; }
        }

        /// <summary>
        /// Gets parameter type
        /// </summary>
        public Type ParameterType
        {
            get { return parameterType; }
        }

        /// <summary>
        /// Gets the XML schema type of the parameter.
        /// </summary>
        /// <value>The XML schema type of the parameter.</value>
        public string ParameterXsdType
        {
            get { return parameterXsdType; }
        }

        private readonly string parameterName;
        private readonly int parameterOrder;
        private readonly Type parameterType;
        private readonly string parameterXsdType;
    }
}
