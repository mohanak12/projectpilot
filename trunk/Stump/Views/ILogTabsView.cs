namespace Stump.Views
{
    public interface ILogTabsView
    {
        void AddTab(LogTabData logTabData);

        void SwitchToLog(int logIndex);
    }
}