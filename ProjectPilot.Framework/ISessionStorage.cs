namespace ProjectPilot.Framework
{
    /// <summary>
    /// Represents an object which stores <see cref="ISessionState"/> objects.
    /// </summary>
    public interface ISessionStorage
    {
        /// <summary>
        /// Loads the session for the specified session holder.
        /// </summary>
        /// <param name="sessionHolderId">The unique ID of the session holder.</param>
        /// <returns><see cref="ISessionState"/> object loaded from the storage.
        /// If the session holder does not have the session state stored, this method
        /// return an empty session state.</returns>
        ISessionState LoadSession(string sessionHolderId);

        /// <summary>
        /// Saves the session state to the storage.
        /// </summary>
        /// <param name="sessionState">Session state to be saved.</param>
        void SaveSession(ISessionState sessionState);
    }
}