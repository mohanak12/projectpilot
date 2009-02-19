using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;
using Stump.Models;

namespace Stump.Services
{
    public class DefaultWorkspaceStorage : IWorkspaceStorage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWorkspaceStorage"/> class.
        /// </summary>
        /// <param name="workspaceFileName">Name of the workspace file.</param>
        public DefaultWorkspaceStorage(string workspaceFileName)
        {
            this.workspaceFileName = workspaceFileName;
        }

        /// <summary>
        /// Loads the workspace from the disk.
        /// </summary>
        /// <returns>
        /// A <see cref="Workspace"/> object. If the workspace is not available or it cannot be read,
        /// simply return an empty workspace.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public Workspace LoadWorkspace()
        {
            if (false == File.Exists(workspaceFileName))
                return new Workspace();

            try
            {
                using (Stream stream = File.Open(workspaceFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    Workspace workspace = (Workspace) formatter.Deserialize(stream);
                    return workspace;
                }
            }
            catch (Exception)
            {
                return new Workspace();
            }
        }

        /// <summary>
        /// Saves the workspace to the disk.
        /// </summary>
        /// <param name="workspace">The workspace to be saved.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SaveWorkspace(Workspace workspace)
        {
            try
            {
                using (Stream stream = File.Open(workspaceFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, workspace);
                }
            }
            catch (Exception ex)
            {
                log.Error("Workspace file could not be saved.", ex);
                // do nothing else
            }
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(DefaultWorkspaceStorage));
        private readonly string workspaceFileName;
    }
}