namespace Headless.Threading
{
    public interface IWorkerMonitor
    {
        void SendReport(string threadId, WorkerActivityType activityType, string message, params object[] args);
    }
}