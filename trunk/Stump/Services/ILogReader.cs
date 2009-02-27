namespace Stump.Services
{
    public interface ILogReader
    {
        string FetchLogContents(string logFileName);
    }
}