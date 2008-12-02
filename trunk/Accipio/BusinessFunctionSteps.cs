using System.Collections.Generic;

namespace Accipio
{
    public class BusinessFunctionSteps
    {
        public BusinessFunctionSteps(IList<string> runActions)
        {
            RunActions = runActions;
        }

        public IList<string> RunActions { get; private set; }
    }
}
