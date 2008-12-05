
namespace Accipio
{
    /// <summary>
    /// Saves business action parameter name and parameter type
    /// </summary>
    public class BusinessActionParameters
    {
        /// <summary>
        /// Initializes a new instance of the BusinessActionParameters class.
        /// </summary>
        /// <param name="parameterName">Name of parameter</param>
        /// <param name="parameterType">Type of parameter</param>
        public BusinessActionParameters(string parameterName, string parameterType)
        {
            this.parameterName = parameterName;
            this.parameterType = parameterType;
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
        public string ParameterType
        {
            get { return parameterType; }
        }

        private readonly string parameterName;
        private readonly string parameterType;
    }
}
