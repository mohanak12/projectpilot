using Stump.Models;

namespace Stump.Services
{
    /// <summary>
    /// Holds workspaces on the disk.
    /// </summary>
    public interface IWorkspaceStorage
    {
        /// <summary>
        /// Loads the workspace from the disk.  
        /// </summary>
        /// <returns>A <see cref="Workspace"/> object. If the workspace is not available or it cannot be read,
        /// simply return an empty workspace.</returns>
        Workspace LoadWorkspace();

        /// <summary>
        /// Saves the workspace to the disk.
        /// </summary>
        /// <param name="workspace">The workspace to be saved.</param>
        void SaveWorkspace(Workspace workspace);
    }
}