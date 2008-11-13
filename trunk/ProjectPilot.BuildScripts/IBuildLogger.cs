namespace ProjectPilot.BuildScripts
{
    public interface IBuildLogger
    {
        void Log (string format, params object[] args);
    }
}