using System;
using System.Globalization;
using System.Threading;
using Headless.Threading;
using MbUnit.Framework;

namespace ProjectPilot.Tests.HeadlessTests
{
    [TestFixture]
    public class WorkerTests
    {
        [Test]
        [Row(20, 3)]
        [Row(3, 15)]
        public void Test(int threads, int jobs)
        {
            IWorkerMonitor workerMonitor = new TestWorkerMonitor();
            IThreadFactory threadFactory = new DefaultThreadFactory();

            using (ManualResetEvent stopSignal = new ManualResetEvent(false))
            {
                using (JobQueue<Job> queue = new JobQueue<Job>("Worker queue", stopSignal))
                {
                    for (int i = 0; i < threads; i++)
                    {
                        TestWorker worker = new TestWorker (i.ToString(CultureInfo.InvariantCulture), queue, threadFactory, workerMonitor);
                        queue.AddWorker(worker);
                    }

                    queue.StartWorkers();

                    for (int i = 0; i < jobs; i++)
                    {
                        Job job = new Job("0");
                        queue.Enqueue(job);
                    }

                    Assert.IsTrue(queue.WaitForQueueToEmpty(TimeSpan.FromSeconds(10)));

                    queue.AssertAllThreadsAlive();

                    stopSignal.Set();

                    threadFactory.WaitForAllThreadsToStop(TimeSpan.FromSeconds(5));

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
