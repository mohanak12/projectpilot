using Headless.Threading;
using log4net;

namespace ProjectPilot.Tests.HeadlessTests
{
    public class TestWorkerMonitor : IWorkerMonitor
    {
        public void SendReport(string threadId, WorkerActivityType activityType, string message, params object[] args)
        {
            log.DebugFormat("{0}, {1}", threadId, activityType);
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(TestWorkerMonitor));
    }
}