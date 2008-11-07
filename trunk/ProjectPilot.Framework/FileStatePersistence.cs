using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Serialization;

namespace ProjectPilot.Framework
{
    /// <summary>
    /// An implementation of <see cref="IStatePersistence"/> that uses files to store
    /// the state.
    /// </summary>
    public class FileStatePersistence : IStatePersistence
    {
        public FileStatePersistence(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public T LoadState<T>(string projectId, string moduleId, string objectId)
            where T : class
        {
            string fullFileName = ConstructStateFileName(projectId, moduleId, objectId, false);

            if (false == fileManager.FileExists (fullFileName))
                return null;

            T stateObject = fileManager.DeserializeFromXmlFile<T>(fullFileName);

            return stateObject;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void SaveState<T>(string projectId, string moduleId, string objectId, T state) where T : class
        {
            string fullFileName = ConstructStateFileName(projectId, moduleId, objectId, true);

            fileManager.SerializeIntoXmlFile<T>(fullFileName, state);
        }

        private string ConstructStateFileName(
            string projectId, 
            string moduleId, 
            string objectId,
            bool ensurePathExists)
        {
            return fileManager.GetProjectFullFileName(
                projectId,
                moduleId,
                string.Format(CultureInfo.InvariantCulture, "{0}.{1}", objectId, "state"),
                ensurePathExists);
        }

        private readonly IFileManager fileManager;
    }
}