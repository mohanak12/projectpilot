using System.Threading;
using Castle.Core;
using Castle.Windsor;
using MbUnit.Framework;
using ProjectPilot.Framework;
using ProjectPilot.Framework.Modules;
using ProjectPilot.Framework.Runners;

namespace ProjectPilot.Tests.Framework.Runners
{
    [TestFixture]
    public class DefaultRunnerTests
    {
        [Test]
        public void StartAndStopRunner()
        {   
            WindsorContainer windsorContainer = new WindsorContainer(@"SampleConfigurations\ProjectPilot.config.xml");
            windsorContainer.Kernel.ComponentCreated += new Castle.MicroKernel.ComponentInstanceDelegate(Kernel_ComponentCreated);
            windsorContainer.Kernel.ComponentModelCreated += new Castle.MicroKernel.ComponentModelDelegate(Kernel_ComponentModelCreated);
            windsorContainer.Kernel.ComponentRegistered += new Castle.MicroKernel.ComponentDataDelegate(Kernel_ComponentRegistered);
            windsorContainer.Kernel.DependencyResolving += new Castle.MicroKernel.DependencyDelegate(Kernel_DependencyResolving);
            windsorContainer.Kernel.HandlerRegistered += new Castle.MicroKernel.HandlerDelegate(Kernel_HandlerRegistered);

            WindsorContainerGraphs.GenerateDependencyGraph(windsorContainer, "graph.dot");

            using (IRunner runner = windsorContainer.Resolve<IRunner>())
            {
                Project[] projects = windsorContainer.Kernel.ResolveServices<Project>();

                IProjectRegistry projectRegistry = windsorContainer.Resolve<IProjectRegistry>();
                Assert.AreEqual(1, projectRegistry.ProjectsCount);

                Project project = projectRegistry.GetProject("bhwr");
                Assert.IsNotNull(project);

                Assert.AreEqual(1, project.ModulesCount);
                IProjectModule module = project.GetModule("RevisionControlStats");

                runner.Start();
                Thread.Sleep(5000);
            }
        }

        void Kernel_HandlerRegistered(Castle.MicroKernel.IHandler handler, ref bool stateChanged)
        {
            System.Console.Out.WriteLine("HandlerRegistered: {0}", handler.ComponentModel.Name);
        }

        void Kernel_DependencyResolving(ComponentModel client, DependencyModel model, object dependency)
        {
            System.Console.Out.WriteLine("DependencyResolving: {0} -> {1}", client.Name, model.DependencyKey);
        }

        void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            System.Console.Out.WriteLine("ComponentRegistered: {0}", key);
        }

        void Kernel_ComponentModelCreated(ComponentModel model)
        {
            System.Console.Out.WriteLine("ComponentModelCreated: {0}", model.Name);
        }

        void Kernel_ComponentCreated(ComponentModel model, object instance)
        {
            System.Console.Out.WriteLine("ComponentCreated: {0}", model.Name);
        }
    }
}
