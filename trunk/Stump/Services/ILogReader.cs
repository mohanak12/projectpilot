namespace Stump.Services
{
    public delegate void LogContentsFetchedCallback(string logContents);

    public interface ILogReader
    {
        void FetchLogContents(string logFileName, LogContentsFetchedCallback logContentsFetchedCallback);
    }
}