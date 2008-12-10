namespace Flubu.BuildRunner
{
    public interface IBuildLogger
    {
        void Log (string format, params object[] args);
    }
}