using Headless;
using Headless.Configuration;
using Headless.Threading;
using Rhino.Mocks;

namespace ProjectPilot.Tests.HeadlessTests
{
    public class HeadlessMother
    {
        public HeadlessMother()
        {
            projectRegistry = MockRepository.GenerateMock<IProjectRegistry>();
            workerMonitor = new DefaultWorkerMonitor();

            ServiceInfo serviceInfo = new ServiceInfo();
            serviceInfo.ComputerName = "computer";
            serviceInfo.PortNumber = 3434;

            projectRegistry.Expect(r => r.ListProjects()).Return(new string[] { "ProjectPilot", "Headless", "Flubu" })
                .Repeat.Any();

            Project project;
            project = new Project("ProjectPilot");
            project.ProjectRegistry = projectRegistry;
            projectRegistry.Expect(r => r.GetProject("ProjectPilot")).Return(project).Repeat.Any();

            project = new Project("Headless");
            ITrigger headlessTrigger = MockRepository.GenerateStub<ITrigger>();
            headlessTrigger.Stub(t => t.IsTriggered()).Callback(() => triggerCounter++ == 0).Return(true).Repeat.Any();
            project.Triggers.Add(headlessTrigger);
            project.ProjectRegistry = projectRegistry;
            projectRegistry.Expect(r => r.GetProject("Headless")).Return(project).Repeat.Any();

            project = new Project("Flubu");
            project.ProjectRegistry = projectRegistry;
            projectRegistry.Expect(r => r.GetProject("Flubu")).Return(project).Repeat.Any();
        }

        public IProjectRegistry ProjectRegistry
        {
            get { return projectRegistry; }
        }

        public IWorkerMonitor WorkerMonitor
        {
            get { return workerMonitor; }
        }

        private IProjectRegistry projectRegistry;
        private int triggerCounter;
        private IWorkerMonitor workerMonitor;
    }
}