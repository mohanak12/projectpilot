using System;
using System.Collections.Generic;

namespace Accipio
{
    /// <summary>
    /// Saves business action id and action parameters
    /// </summary>
    public class BusinessAction
    {
        /// <summary>
        /// Initializes a new instance of the BusinessAction class.
        /// </summary>
        /// <param name="actionId">Id of business action</param>
        public BusinessAction(string actionId)
        {
            ActionName = actionId;
        }

        /// <summary>
        /// Gets the business action name.
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// Gets or sets business action description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the count of business action's parameters.
        /// </summary>
        /// <value>The parameters count.</value>
        public int ParametersCount
        {
            get { return parameters.Count; }
        }

        /// <summary>
        /// Add business action parameters
        /// </summary>
        /// <param name="parameter">The parameter to add.</param>
        public void AddParameter(BusinessActionParameter parameter)
        {
            parameters.Add(parameter.ParameterName, parameter);
        }

        public IEnumerable<BusinessActionParameter> EnumerateParameters()
        {
            List<BusinessActionParameter> parametersOrdered = new List<BusinessActionParameter>(parameters.Values);
            parametersOrdered.Sort(delegate (BusinessActionParameter parameter1, BusinessActionParameter parameter2)
                                       {
                                           return parameter1.ParameterOrder.CompareTo(parameter2.ParameterOrder);
                                       });
            return parametersOrdered;
        }

        public BusinessActionParameter GetParameter(string parameterName)
        {
            return parameters[parameterName];
        }

        private readonly Dictionary<string, BusinessActionParameter> parameters = new Dictionary<string, BusinessActionParameter>();
    }
}
