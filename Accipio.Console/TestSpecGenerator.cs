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
            CheckForValidInputArguments(args);
        }

        private static void CheckForValidInputArguments(string[] args)
        {
            for (int i = 1; i<args.Length-1;i++ )
            {
                FileInfo fileInfo = new FileInfo(args[i]);
                if (!File.Exists(fileInfo.FullName))
                {
                    throw new FileNotFoundException("File not found!");
                }
            }
        }

        public void Process()
        {
            throw new System.NotImplementedException();
        }
    }
}