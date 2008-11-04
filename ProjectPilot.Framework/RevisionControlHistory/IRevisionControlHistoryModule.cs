using ProjectPilot.Framework.Modules;

namespace ProjectPilot.Framework.RevisionControlHistory
{
    public interface IRevisionControlHistoryModule : IProjectModule
    {
        RevisionControlHistoryData FetchHistory();
    }
}