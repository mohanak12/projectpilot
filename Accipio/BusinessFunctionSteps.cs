using System.Collections.Generic;

namespace Accipio
{
    /// <summary>
    /// Saves function steps
    /// </summary>
    public class BusinessFunctionSteps
    {
        /// <summary>
        /// Initializes a new instance of the BusinessFunctionSteps class.
        /// </summary>
        /// <param name="runActions">Action name</param>
        public BusinessFunctionSteps(IList<string> runActions)
        {
            RunActions = runActions;
        }

        /// <summary>
        /// Gets actions to be run
        /// </summary>
        public IList<string> RunActions { get; private set; }
    }
}
