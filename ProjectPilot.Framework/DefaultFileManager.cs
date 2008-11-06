using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace ProjectPilot.Framework
{
    public class DefaultFileManager : IFileManager
    {
        public DefaultFileManager(
            string storageRootDir,
            IProjectPilotConfiguration configuration)
        {
            this.storageRootDir = storageRootDir;
            this.configuration = configuration;
        }

        public DefaultFileManager(
            string storageRootDir, 
            IProjectPilotConfiguration configuration,
            IProjectRegistry projectRegistry)
            : this (storageRootDir, configuration)
        {
            this.projectRegistry = projectRegistry;
        }

        public IProjectPilotConfiguration Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        public IProjectRegistry ProjectRegistry
        {
            get { return projectRegistry; }
            set { projectRegistry = value; }
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

        /// <summary>
        /// Translates the relative file name of a file within ProjectPilot Web application's root to an URL through
        /// which this file can be accessed via Web.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>
        /// An URL through which this file can be accessed via Web
        /// </returns>
        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        public Uri TranslateToUrl(string fileName)
        {
            return new Uri(new Uri(configuration.ProjectPilotWebAppRootUrl), fileName);
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

        private IProjectPilotConfiguration configuration;
        private IProjectRegistry projectRegistry;
        private string storageRootDir;
    }
}