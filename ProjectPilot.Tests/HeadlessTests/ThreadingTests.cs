using System;
using System.Threading;
using Headless;
using Headless.Threading;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.HeadlessTests
{
    [TestFixture, Pending("TODO: Igor")]
    public class ThreadingTests
    {
        [Test]
        public void Run()
        {
            IProjectRegistryProvider projectRegistryProvider = MockRepository.GenerateMock<IProjectRegistryProvider>();
            IWorkerMonitor workerMonitor = MockRepository.GenerateMock<IWorkerMonitor>();

            using (ManualResetEvent stopSignal = new ManualResetEvent(false))
            {
                using (CheckTriggersQueueFeeder checkTriggersQueueFeeder = new CheckTriggersQueueFeeder(
                    stopSignal, 
                    workerMonitor,
                    projectRegistryProvider))
                {
                    checkTriggersQueueFeeder.Start();
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    stopSignal.Set();
                }
            }
        }
    }
}
