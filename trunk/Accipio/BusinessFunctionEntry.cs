using System.Collections.Generic;

namespace Accipio
{
    /// <summary>
    /// Saves business function and function steps
    /// </summary>
    public class BusinessFunctionEntry
    {
        /// <summary>
        /// Initializes a new instance of the BusinessFunctionEntry class.
        /// </summary>
        /// <param name="functionId">Id of function</param>
        public BusinessFunctionEntry(string functionId)
        {
            FunctionId = functionId;
        }

        /// <summary>
        /// Adds function step to list of steps
        /// </summary>
        /// <param name="runActions">Actions name</param>
        public void AddFunctionStep(IList<string> runActions)
        {
            steps.Add(new BusinessFunctionSteps(runActions));
        }

        /// <summary>
        /// Gets business action id.
        /// </summary>
        public string FunctionId { get; private set; }

        /// <summary>
        /// Gets list of all function steps
        /// </summary>
        public IList<BusinessFunctionSteps> Steps
        {
            get { return steps; }
        }

        private readonly List<BusinessFunctionSteps> steps = new List<BusinessFunctionSteps>();
    }
}
