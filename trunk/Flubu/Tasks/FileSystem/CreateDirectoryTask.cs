using System;
using System.IO;

namespace Flubu.Tasks.FileSystem
{
    public class CreateDirectoryTask : TaskBase
    {
        public string DirectoryPath
        {
            get { return directoryPath; }
            set { directoryPath = value; }
        }

        public override string TaskDescription
        {
            get
            {
                return String.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Create directory '{0}'", 
                    directoryPath);
            }
        }

        public CreateDirectoryTask (string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public static void Execute(IScriptExecutionEnvironment environment, string directoryPath)
        {
            CreateDirectoryTask task = new CreateDirectoryTask (directoryPath);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            Directory.CreateDirectory (directoryPath);
        }

        private string directoryPath;
    }
}
