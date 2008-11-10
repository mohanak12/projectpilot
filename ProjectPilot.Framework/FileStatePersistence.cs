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

        public string ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public string ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }

        public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public T LoadState<T>()
            where T : class
        {
            string fullFileName = ConstructStateFileName(false);

            if (false == fileManager.FileExists (fullFileName))
                return null;

            T stateObject = fileManager.DeserializeFromXmlFile<T>(fullFileName);

            return stateObject;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public void SaveState<T>(T state) where T : class
        {
            string fullFileName = ConstructStateFileName(true);

            fileManager.SerializeIntoXmlFile<T>(fullFileName, state);
        }

        private string ConstructStateFileName(bool ensurePathExists)
        {
            return fileManager.GetProjectFullFileName(
                projectId,
                moduleId,
                string.Format(CultureInfo.InvariantCulture, "{0}.{1}", objectId, "state"),
                ensurePathExists);
        }

        private readonly IFileManager fileManager;
        private string moduleId;
        private string objectId;
        private string projectId;
    }
}