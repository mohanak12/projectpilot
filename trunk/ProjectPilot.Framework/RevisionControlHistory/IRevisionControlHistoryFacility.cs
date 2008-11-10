using ProjectPilot.Framework.Modules;

namespace ProjectPilot.Framework.RevisionControlHistory
{
    public interface IRevisionControlHistoryFacility
    {
        RevisionControlHistoryData FetchHistory();
    }
}