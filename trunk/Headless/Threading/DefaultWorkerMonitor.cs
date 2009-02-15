using System.Globalization;
using log4net;

namespace Headless.Threading
{
    public class DefaultWorkerMonitor : IWorkerMonitor
    {
        public void SendReport(string threadId, WorkerActivityType activityType, string message, params object[] args)
        {
            if (log.IsDebugEnabled)
            {
                string formattedMessage;
                if (args.Length > 0)
                    formattedMessage = string.Format(
                        CultureInfo.InvariantCulture,
                        message,
                        args);
                else
                    formattedMessage = message;

                log.DebugFormat(
                    "Thread '{0}' - activity {1}: '{2}'",
                    threadId,
                    activityType,
                    formattedMessage);
            }
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(DefaultWorkerMonitor));
    }
}