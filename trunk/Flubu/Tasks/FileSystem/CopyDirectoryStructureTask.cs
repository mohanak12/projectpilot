using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Flubu.Tasks.FileSystem
{
    /// <summary>
    /// Copies a directory tree from the source to the destination.
    /// </summary>
    public class CopyDirectoryStructureTask : TaskBase
    {
        /// <summary>
        /// Gets the task description.
        /// </summary>
        /// <value>The task description.</value>
        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Copy directory structure from '{0}' to '{1}", 
                    sourcePath, 
                    destinationPath);
            }
        }

        /// <summary>
        /// Gets or sets the exclusion pattern for files.
        /// </summary>
        /// <remarks>All files whose paths match this regular expression
        /// will not be copied. If the <see cref="ExclusionPattern"/> is <c>null</c>, it will be ignored.</remarks>
        /// <value>The exclusion pattern.</value>
        public string ExclusionPattern
        {
            get { return exclusionPattern; }
            set { exclusionPattern = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyDirectoryStructureTask"/> class
        /// using a specified source and destination path and an indicator whether to overwrite existing files.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="overwriteExisting">if set to <c>true</c> the task will overwrite existing destination files.</param>
        public CopyDirectoryStructureTask (string sourcePath, string destinationPath, bool overwriteExisting)
        {
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.overwriteExisting = overwriteExisting;
        }

        /// <summary>
        /// Copies a directory tree from the source to the destination.
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="overwriteExisting">if set to <c>true</c> the task will overwrite existing destination files.</param>
        public static void Execute(
            IScriptExecutionEnvironment environment,
            string sourcePath, 
            string destinationPath, 
            bool overwriteExisting)
        {
            CopyDirectoryStructureTask task = new CopyDirectoryStructureTask (sourcePath, destinationPath, overwriteExisting);
            task.Execute (environment);
        }

        /// <summary>
        /// Internal task execution code.
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            Regex exclusionRegex = null;
            if (exclusionPattern != null)
                exclusionRegex = new Regex (exclusionPattern, RegexOptions.IgnoreCase|RegexOptions.Singleline);

            CopyStructureRecursive (environment, sourcePath, destinationPath, exclusionRegex);
        }

        private void CopyStructureRecursive (
            IScriptExecutionEnvironment environment,
            string sourcePathRecursive, 
            string destinationPathRecursive,
            Regex exclusionRegex)
        {
            if (exclusionRegex != null && exclusionRegex.IsMatch (sourcePathRecursive))
                return;

            DirectoryInfo info = new DirectoryInfo (sourcePathRecursive);

            if (false == Directory.Exists (destinationPathRecursive))
                Directory.CreateDirectory (destinationPathRecursive);

            foreach (FileSystemInfo fileSystemInfo in info.GetFileSystemInfos ())
            {
                if (fileSystemInfo is FileInfo)
                {
                    if (exclusionRegex != null && exclusionRegex.IsMatch (fileSystemInfo.FullName))
                        continue;

                    FileInfo fileInfo = fileSystemInfo as FileInfo;
                    string filePath = Path.Combine (destinationPathRecursive, fileInfo.Name);

                    fileInfo.CopyTo (filePath, overwriteExisting);
                    environment.Logger.Log(
                        "Copied file '{0}' to '{1}'", 
                        fileSystemInfo.FullName,
                        filePath);
                }
                else
                {
                    DirectoryInfo dirInfo = fileSystemInfo as DirectoryInfo;
                    string subdirectoryPath = Path.Combine (
                        destinationPathRecursive, 
                        dirInfo.Name);
                    CopyStructureRecursive (
                        environment, 
                        dirInfo.FullName, 
                        subdirectoryPath, 
                        exclusionRegex);
                }
            }
        }

        private string sourcePath;
        private string destinationPath;
        private bool overwriteExisting;
        private string exclusionPattern;
    }
}
