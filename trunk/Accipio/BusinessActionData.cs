using System.Collections.Generic;

namespace Accipio
{
    /// <summary>
    /// Contains list of business actions and business functions
    /// </summary>
    public class BusinessActionData
    {
        /// <summary>
        /// Gets the specified action.
        /// </summary>
        /// <param name="actionId">The action id.</param>
        /// <returns>Requested action or <c>null</c> See <see cref="BusinessActionEntry"/></returns>
        public BusinessActionEntry GetAction(string actionId)
        {
            foreach (BusinessActionEntry businessActionEntry in actions)
            {
                if (businessActionEntry.ActionId == actionId)
                {
                    return businessActionEntry;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets list of all business actions.
        /// </summary>
        public IList<BusinessActionEntry> Actions
        {
            get { return actions; }
        }

        /// <summary>
        /// Gets list of all business functions.
        /// </summary>
        public IList<BusinessFunctionEntry> Functions
        {
            get { return functions; }
        }

        private readonly List<BusinessActionEntry> actions = new List<BusinessActionEntry>();
        private readonly List<BusinessFunctionEntry> functions = new List<BusinessFunctionEntry>();
    }
}
