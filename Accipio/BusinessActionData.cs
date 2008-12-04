using System.Collections.Generic;

namespace Accipio
{
    /// <summary>
    /// Contains list of business actions and business functions
    /// </summary>
    public class BusinessActionData
    {
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
