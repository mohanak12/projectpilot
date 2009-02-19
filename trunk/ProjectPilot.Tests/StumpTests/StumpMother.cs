using Stump.Models;

namespace ProjectPilot.Tests.StumpTests
{
    public sealed class StumpMother
    {
        public static Workspace CreateWorkspace()
        {
            Workspace workspace = new Workspace();
            workspace.MonitorLogFile(new MonitoredLogFile("d:/log1.txt"));
            workspace.MonitorLogFile(new MonitoredLogFile("d:/log2.txt"));
            workspace.MonitorLogFile(new MonitoredLogFile("d:/log3.txt"));
            return workspace;
        }

        private StumpMother()
        {
        }
    }
}