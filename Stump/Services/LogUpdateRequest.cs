namespace Stump.Services
{
    public class LogUpdateRequest
    {
        public LogUpdateRequest(
            string logFileName,
            LogContentsFetchedCallback logContentsFetchedCallback)
        {
            this.logFileName = logFileName;
            this.logContentsFetchedCallback = logContentsFetchedCallback;
        }

        public string LogContents
        {
            get { return logContents; }
            set { logContents = value; }
        }

        public LogContentsFetchedCallback LogContentsFetchedCallback
        {
            get { return logContentsFetchedCallback; }
        }

        public string LogFileName
        {
            get { return logFileName; }
        }

        private string logContents;
        private string logFileName;
        private LogContentsFetchedCallback logContentsFetchedCallback;
    }
}