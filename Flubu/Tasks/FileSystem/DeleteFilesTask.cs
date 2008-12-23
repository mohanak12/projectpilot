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

        public DeleteFilesTask (string directoryPath, string filePattern, bool recursive)
        {
            this.directoryPath = directoryPath;
            this.filePattern = filePattern;
            this.recursive = recursive;
        }

        public static void Execute(
            IScriptExecutionEnvironment environment,
            string directoryPath,
            string filePattern,
            bool recursive)
        {
            DeleteFilesTask task = new DeleteFilesTask (directoryPath, filePattern, recursive);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            SearchOption searchOption = SearchOption.TopDirectoryOnly;
            if (recursive)
                searchOption = SearchOption.AllDirectories;

            foreach (string file in Directory.GetFiles (directoryPath, filePattern, searchOption))
            {
                File.Delete (file);
                environment.LogMessage("Deleted file '{0}'", file);
            }
        }

        private string directoryPath;
        private string filePattern;
        private bool recursive;
    }
}
