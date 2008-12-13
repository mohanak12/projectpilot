using System;
using System.IO;

namespace Flubu.Tasks.FileSystem
{
    public class DeleteFilesTask : TaskBase
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
                    "Delete files from directory {0} matching pattern '{1}'", 
                    directoryPath, 
                    filePattern);
            }
        }

        public DeleteFilesTask (string directoryPath, string filePattern)
        {
            this.directoryPath = directoryPath;
            this.filePattern = filePattern;
        }

        public static void Execute(
            IScriptExecutionEnvironment environment,
            string directoryPath,
            string filePattern)
        {
            DeleteFilesTask task = new DeleteFilesTask (directoryPath, filePattern);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            foreach (string file in Directory.GetFiles (directoryPath, filePattern))
            {
                File.Delete (file);
                environment.LogMessage("Deleted file '{0}'", file);
            }
        }

        private string directoryPath;
        private string filePattern;
    }
}
