using System;
using System.Threading;
using Headless;
using Headless.Configuration;
using Headless.Threading;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.HeadlessTests
{
    [TestFixture]
    public class ThreadingTests
    {
        [Test]
        public void CheckTriggersQueue()
        {
            HeadlessMother mother = new HeadlessMother();
            IThreadFactory threadFactory = new DefaultThreadFactory();
            IWorkerMonitor workerMonitor = mother.WorkerMonitor;

            using (ManualResetEvent stopSignal = new ManualResetEvent(false))
            {
                JobQueue<ProjectRelatedJob> buildQueue = new JobQueue<ProjectRelatedJob>("buildQueue", stopSignal);

                using (CheckTriggersQueueFeeder checkTriggersQueueFeeder = new CheckTriggersQueueFeeder(
                    stopSignal, 
                    buildQueue,
                    threadFactory,
                    workerMonitor,
                    mother.ProjectRegistry))
                {
                    checkTriggersQueueFeeder.Start();
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    stopSignal.Set();

                    threadFactory.WaitForAllThreadsToStop(TimeSpan.FromSeconds(5));
                }
            }

            mother.ProjectRegistry.AssertWasCalled(r => r.ChangeProjectStatus("ProjectPilot", ProjectStatus.CheckingTriggers));
            mother.ProjectRegistry.AssertWasCalled(r => r.ChangeProjectStatus("Headless", ProjectStatus.CheckingTriggers));
            mother.ProjectRegistry.AssertWasCalled(r => r.ChangeProjectStatus("Flubu", ProjectStatus.CheckingTriggers));
            mother.ProjectRegistry.VerifyAllExpectations();
        }

        [Test]
        public void HeadlessServiceTest()
        {
            HeadlessMother mother = new HeadlessMother();
            IThreadFactory threadFactory = new DefaultThreadFactory();
            IWorkerMonitor workerMonitor = mother.WorkerMonitor;

            mother.ProjectRegistry.Expect(r => r.ChangeProjectStatus("Headless", ProjectStatus.CheckingTriggers)).Repeat
                .Any();

            using (HeadlessService service = new HeadlessService(mother.ProjectRegistry, threadFactory, workerMonitor))
            {
                service.Start();
                Thread.Sleep(TimeSpan.FromSeconds(20));
                service.Stop(TimeSpan.FromSeconds(5));
            }

            mother.ProjectRegistry.AssertWasCalled(r => r.ChangeProjectStatus("Headless", ProjectStatus.Sleeping));
            mother.ProjectRegistry.AssertWasNotCalled(r => r.ChangeProjectStatus("ProjectPilot", ProjectStatus.Building));
            mother.ProjectRegistry.AssertWasNotCalled(r => r.ChangeProjectStatus("Flubu", ProjectStatus.Building));
            mother.ProjectRegistry.VerifyAllExpectations();
        }

        /// <summary>Test fixture setup code.</summary>
        [FixtureSetUp]
        public void FixtureSetup()
        {
            log4net.Config.XmlConfigurator.Configure();            
        }
    }
}
