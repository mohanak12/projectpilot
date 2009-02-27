using System.Collections.Generic;
using Rhino.Mocks;
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

            logReader.Expect(lr => lr.FetchLogContents("d:/log1.txt")).Return("logContents1");
            logReader.Expect(lr => lr.FetchLogContents("d:/log2.txt")).Return("logContents2");
            logReader.Expect(lr => lr.FetchLogContents("d:/log3.txt")).Return("logContents2");

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