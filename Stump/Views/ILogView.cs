namespace Stump.Views
{
    public interface ILogView
    {
        bool IsLogDisplayActive { get; }

        bool MonitoringEnabled { get; set; }

        void IndicateLogFileDeleted();
        
        void IndicateLogFileUpdated();

        void ShowLogContents(string logContents);
    }
}