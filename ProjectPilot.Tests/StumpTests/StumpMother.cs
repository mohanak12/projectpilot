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
            logReader = MockRepository.GenerateStub<ILogReader>();

            for (int i = 0; i < 3; i++)
            {
                logMonitors.Add(MockRepository.GenerateMock<ILogMonitor>());
            }
        }

        public IList<ILogMonitor> LogMonitors
        {
            get { return logMonitors; }
        }

        public ILogReader LogReader
        {
            get { return logReader; }
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

        private readonly ILogReader logReader;
        private List<ILogMonitor> logMonitors = new List<ILogMonitor>();
        private Workspace workspace;
    }
}