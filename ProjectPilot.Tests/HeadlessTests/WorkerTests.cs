using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Headless.Threading;
using MbUnit.Framework;
using Rhino.Mocks;

namespace ProjectPilot.Tests.HeadlessTests
{
    [TestFixture]
    public class WorkerTests
    {
        [Test]
        public void Test()
        {
            IWorkerMonitor workerMonitor = new TestWorkerMonitor();

            using (ManualResetEvent stopSignal = new ManualResetEvent(false))
            {
                using (JobQueue<Job> queue = new JobQueue<Job>(stopSignal))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        TestWorker worker = new TestWorker (i.ToString(CultureInfo.InvariantCulture), queue, workerMonitor);
                        queue.AddWorker(worker);
                    }

                    queue.StartWorkers();

                    for (int i = 0; i < 7; i++)
                    {
                        Job job = new Job("0");
                        queue.Enqueue(job);
                    }

                    Assert.IsTrue(queue.WaitForQueueToEmpty(TimeSpan.FromSeconds(10)));

                    stopSignal.Set();

                    queue.WaitForWorkersToStop(TimeSpan.FromSeconds(5));

                    Assert.IsTrue(queue.IsEmpty);
                }
            }
         }

        [FixtureSetUp]
        public void FixtureSetup()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
