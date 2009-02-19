using System.IO;
using MbUnit.Framework;
using Rhino.Mocks;
using Stump.Models;
using Stump.Services;

namespace ProjectPilot.Tests.StumpTests
{
    [TestFixture]
    public class WorkspaceTests
    {
        [Test]
        public void DoNotFailIfWorkspaceFileCannotBeRead()
        {
            const string WorkspaceFileName = "workspace.dat";
            IWorkspaceStorage workspaceStorage = new DefaultWorkspaceStorage(WorkspaceFileName);

            // save some garbage to the file
            File.WriteAllText(WorkspaceFileName, "blablabla");

            Workspace workspace = workspaceStorage.LoadWorkspace();
            Assert.IsNotNull(workspace);
        }

        [Test]
        public void DoNotFailIfWorkspaceFileDoesNotExist()
        {
            const string WorkspaceFileName = "workspace.dat";
            IWorkspaceStorage workspaceStorage = new DefaultWorkspaceStorage(WorkspaceFileName);

            // make sure a file does not exist
            if (File.Exists(WorkspaceFileName))
                File.Delete(WorkspaceFileName);

            Workspace workspace = workspaceStorage.LoadWorkspace();
            Assert.IsNotNull(workspace);
        }

        [Test]
        public void LoadSavedWorkspace()
        {
            Workspace workspace = new Workspace();
            workspace.MonitorLogFile(new MonitoredLogFile("TestLog.txt"));

            const string WorkspaceFileName = "workspace.dat";
            IWorkspaceStorage workspaceStorage = new DefaultWorkspaceStorage(WorkspaceFileName);

            // save some garbage to the file
            File.WriteAllText(WorkspaceFileName, "blablabla");

            workspaceStorage.SaveWorkspace(workspace);

            Workspace loadedWorkspace = workspaceStorage.LoadWorkspace();

            Assert.AreEqual(workspace.LogFiles.Count, loadedWorkspace.LogFiles.Count);
        }

        /// <summary>
        /// The application should load the workspace at its startup.
        /// </summary>
        [Test]
        public void LoadWorkspaceAtStartup()
        {
            IWorkspaceStorage workspaceStorage = MockRepository.GenerateMock<IWorkspaceStorage>();
            Workspace workspace = new Workspace();

            workspaceStorage.Expect(ws => ws.LoadWorkspace()).Return(workspace);

            StumpApplication app = new StumpApplication(workspaceStorage);
            app.Start();

            Assert.IsNotNull(app.Workspace);
            workspaceStorage.VerifyAllExpectations();
        }

        /// <summary>
        /// The application should save the current workspace before its shutdown.
        /// </summary>
        [Test]
        public void SaveWorkspaceAtShutdown()
        {
            IWorkspaceStorage workspaceStorage = MockRepository.GenerateMock<IWorkspaceStorage>();

            StumpApplication app = new StumpApplication(workspaceStorage);

            workspaceStorage.Expect(ws => ws.SaveWorkspace(app.Workspace));

            app.Shutdown();

            workspaceStorage.VerifyAllExpectations();            
        }
    }
}
