using System.Diagnostics.CodeAnalysis;

namespace Stump.Services
{
    public delegate void LogContentsFetchedCallback(LogUpdateRequest logUpdateRequest);

    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public interface ILogUpdaterQueue
    {
        void FetchLogContents(string logFileName, LogContentsFetchedCallback logContentsFetchedCallback);
    }
}