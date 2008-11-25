using System;
using System.IO;

namespace Flubu.Tasks.FileSystem
{
    public class DeleteDirectoryTask : TaskBase
    {
        /// <summary>
        /// Gets the task description.
        /// </summary>
        /// <value>The task description.</value>
        public override string TaskDescription
        {
            get
            {
                return String.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Delete directory '{0}'", 
                    directoryPath);
            }
        }

        public DeleteDirectoryTask (string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public static void Execute(
            IScriptExecutionEnvironment environment,
            string directoryPath)
        {
            DeleteDirectoryTask task = new DeleteDirectoryTask (directoryPath);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            Directory.Delete (directoryPath, true);
        }

        private string directoryPath;
    }
}
