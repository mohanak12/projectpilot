using Stump.Services;

namespace Stump.Models
{
    public class StumpApplication
    {
        public StumpApplication(IWorkspaceStorage workspaceStorage)
        {
            this.workspaceStorage = workspaceStorage;
        }

        public Workspace Workspace
        {
            get { return workspace; }
        }

        public void Shutdown()
        {
            workspaceStorage.SaveWorkspace(workspace);
        }

        public void Start()
        {
            workspace = workspaceStorage.LoadWorkspace();
        }

        private Workspace workspace;

        private readonly IWorkspaceStorage workspaceStorage;
    }
}