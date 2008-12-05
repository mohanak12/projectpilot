using System.Collections.Generic;

namespace Accipio
{
    /// <summary>
    /// Saves business action id and action parameters
    /// </summary>
    public class BusinessActionEntry
    {
        /// <summary>
        /// Initializes a new instance of the BusinessActionEntry class.
        /// </summary>
        /// <param name="actionId">Id of business action</param>
        public BusinessActionEntry(string actionId)
        {
            ActionId = actionId;
        }

        /// <summary>
        /// Gets business action id.
        /// </summary>
        public string ActionId { get; private set; }

        /// <summary>
        /// Gets all business action parameters.
        /// </summary>
        public IList<BusinessActionParameters> ActionParameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Add business action parameters
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterType">Parameter type</param>
        public void AddParameter(string parameterName, string parameterType)
        {
            parameters.Add(new BusinessActionParameters(parameterName, parameterType));
        }

        /// <summary>
        /// Gets or sets business action description.
        /// </summary>
        public string Description { get; set; }

        private readonly List<BusinessActionParameters> parameters = new List<BusinessActionParameters>();
    }
}
