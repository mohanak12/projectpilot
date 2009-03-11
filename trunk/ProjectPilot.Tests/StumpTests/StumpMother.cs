using System.Collections.Generic;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Stump.Models;
using Stump.Services;

namespace ProjectPilot.Tests.StumpTests
{
    public class StumpMother
    {
        public StumpMother()
        {
            CreateWorkspace();
            logUpdaterQueue = MockRepository.GenerateStub<ILogUpdaterQueue>();

            for (int i = 0; i < 3; i++)
            {
                logMonitors.Add(MockRepository.GenerateMock<ILogMonitor>());
            }
        }

        public IList<ILogMonitor> LogMonitors
        {
            get { return logMonitors; }
        }

        public ILogUpdaterQueue LogUpdaterQueue
        {
            get { return logUpdaterQueue; }
        }

        public Workspace Workspace
        {
            get { return workspace; }
        }

        private Workspace CreateWorkspace()
        {
            workspace = new Workspace();
            workspace.MonitorLogFile(new MonitoredLogFile("d:/log1.txt"));
            workspace.MonitorLogFile(new MonitoredLogFile("d:/log2.txt"));
            workspace.MonitorLogFile(new MonitoredLogFile("d:/log3.txt"));
            return workspace;
        }

        private readonly ILogUpdaterQueue logUpdaterQueue;
        private List<ILogMonitor> logMonitors = new List<ILogMonitor>();
        private Workspace workspace;
    }
}