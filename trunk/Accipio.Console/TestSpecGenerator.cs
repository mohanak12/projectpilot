using System.IO;

namespace Accipio.Console
{
    /// <summary>
    /// Generating testing source code and documentation.
    /// </summary>
    public class TestSpecGenerator : IGenerator
    {
        public void Parse(string[] args)
        {
            AccipioHelper.CheckForValidInputArguments(args);
        }

        public void Process()
        {
            throw new System.NotImplementedException();
        }
    }
}