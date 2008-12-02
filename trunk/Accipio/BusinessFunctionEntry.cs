using System.Collections.Generic;

namespace Accipio
{
    public class BusinessFunctionEntry
    {
        public BusinessFunctionEntry(string functionId)
        {
            FunctionId = functionId;
        }

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
