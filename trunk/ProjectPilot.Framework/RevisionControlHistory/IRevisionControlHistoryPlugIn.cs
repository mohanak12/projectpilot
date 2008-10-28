using ProjectPilot.Framework.Modules;

namespace ProjectPilot.Framework.RevisionControlHistory
{
    public interface IRevisionControlHistoryPlugIn : IProjectModule
    {
        RevisionControlHistoryData FetchHistory();
    }
}