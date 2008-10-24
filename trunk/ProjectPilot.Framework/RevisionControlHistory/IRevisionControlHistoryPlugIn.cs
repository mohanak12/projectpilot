namespace ProjectPilot.Framework.RevisionControlHistory
{
    public interface IRevisionControlHistoryPlugIn
    {
        RevisionControlHistoryData FetchHistory();
    }
}