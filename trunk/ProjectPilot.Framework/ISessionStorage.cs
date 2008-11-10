namespace ProjectPilot.Framework
{
    public interface ISessionStorage
    {
        ISessionState LoadSession(string sessionHolderId);
        void SaveSession(ISessionState sessionState);
    }
}