using System;
using System.Globalization;
using System.IO;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Framework
{
    public interface IFileManager
    {
        string GetFullFileName(string domain, string localFileName);
        string GetProjectFullFileName(string projectId, string moduleId, string localFileName, bool assertDirPathExists);
        string FetchProjectFile(string projectId, string moduleId, string localFileName);
    }

    public class DefaultFileManager : IFileManager
    {
        public DefaultFileManager(string storageRootDir, IProjectRegistry projectRegistry)
        {
            this.storageRootDir = storageRootDir;
            this.projectRegistry = projectRegistry;
        }

        public string GetFullFileName(string domain, string localFileName)
        {
            string fullFileName = String.Format(CultureInfo.InvariantCulture,
                "{0}{1}{2}",
                domain,
                Path.DirectorySeparatorChar,
                localFileName);
            fullFileName = Path.Combine(storageRootDir, fullFileName);
            return fullFileName;
        }

        public string GetProjectFullFileName(string projectId, string moduleId, string localFileName, bool assertDirPathExists)
        {
            Project project = projectRegistry.GetProject(projectId);

            // construct the file name
            string fullFileName = String.Format(CultureInfo.InvariantCulture,
                "Storage{0}Projects{0}{1}{0}{2}{0}{3}",
                Path.DirectorySeparatorChar,
                projectId,
                moduleId,
                localFileName);
            fullFileName = Path.Combine(storageRootDir, fullFileName);

            if (assertDirPathExists)
                CreateDirectories(fullFileName, true);

            return fullFileName;
        }

        public string FetchProjectFile(string projectId, string moduleId, string localFileName)
        {
            Project project = projectRegistry.GetProject(projectId);

            // construct the file name
            string fullFileName = GetProjectFullFileName(projectId, moduleId, localFileName, false);

            if (File.Exists(fullFileName))
                return File.ReadAllText(fullFileName);
            else
                return null;
        }

        private void CreateDirectories(string filePath, bool includesFileName)
        {
            string filePathWithoutLastLevel = Path.GetDirectoryName(filePath);
            if (false == String.IsNullOrEmpty(filePathWithoutLastLevel))
            {
                if (false == Directory.Exists(filePathWithoutLastLevel))
                    CreateDirectories(filePathWithoutLastLevel, false);

                if (false == includesFileName)
                    Directory.CreateDirectory(filePath);
            }
        }

        private readonly string storageRootDir;
        private readonly IProjectRegistry projectRegistry;
    }
}