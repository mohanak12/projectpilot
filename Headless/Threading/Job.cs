namespace Headless.Threading
{
    public class Job
    {
        public Job(string correlationId)
        {
            this.correlationId = correlationId;
        }

        public string CorrelationId
        {
            get { return correlationId; }
        }

        private string correlationId;
    }
}