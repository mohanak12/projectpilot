using System;
using System.IO;

namespace ProjectPilot.Framework
{
    /// <summary>
    /// An implementation of the <see cref="ISessionStorage"/> interface which saves session state
    /// objects into files.
    /// </summary>
    public class FileSessionStorage : ISessionStorage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSessionStorage"/> class
        /// using the specified <see cref="IFileManager"/>.
        /// </summary>
        /// <param name="fileManager">The file manager to be used to store session state objects.</param>
        public FileSessionStorage(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        /// <summary>
        /// Clears the session state for the specified session holder.
        /// </summary>
        /// <param name="sessionHolderId">The unique ID of the session holder.</param>
        public void ClearSession (string sessionHolderId)
        {
            try
            {
                string sessionFileName = ConstuctSessionFileName(sessionHolderId);
                fileManager.DeleteFile(sessionFileName);
            }
            catch (DirectoryNotFoundException)
            {
                // do nothing
            }
        }

        /// <summary>
        /// Loads the session for the specified session holder.
        /// </summary>
        /// <param name="sessionHolderId">The unique ID of the session holder.</param>
        /// <returns>
        /// <see cref="ISessionState"/> object loaded from the storage.
        /// If the session holder does not have the session state stored, this method
        /// return an empty session state.
        /// </returns>
        public ISessionState LoadSession(string sessionHolderId)
        {
            string sessionFileName = ConstuctSessionFileName(sessionHolderId);
            if (fileManager.FileExists(sessionFileName))
            {
                SessionState sessionState = fileManager.DeserializeFromFile<SessionState>(sessionFileName);
                sessionState.SessionStorage = this;
                return sessionState;
            }

            return new SessionState(sessionHolderId, this);
        }

        /// <summary>
        /// Saves the session state to the storage.
        /// </summary>
        /// <param name="sessionState">Session state to be saved.</param>
        public void SaveSession(ISessionState sessionState)
        {
            string sessionFileName = ConstuctSessionFileName(sessionState.SessionHolderId);
            fileManager.EnsurePathExists(sessionFileName);
            fileManager.SerializeIntoFile(sessionFileName, sessionState);
        }

        private string ConstuctSessionFileName(string sessionHolderId)
        {
            return fileManager.GetFullFileName("Session", sessionHolderId);
        }

        private readonly IFileManager fileManager;
    }
}