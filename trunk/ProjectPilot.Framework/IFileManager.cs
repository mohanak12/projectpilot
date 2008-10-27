using System;
using System.Globalization;
using System.IO;
using ProjectPilot.Framework.Projects;

namespace ProjectPilot.Framework
{
    public interface IFileManager
    {
        string GetFullFileName(string projectId, string localFileName);
        string FetchFile(string projectId, string localFileName);
    }

    public class DefaultFileManager : IFileManager
    {
        public DefaultFileManager(IProjectRegistry projectRegistry)
        {
            this.projectRegistry = projectRegistry;
        }

        public string GetFullFileName(string projectId, string localFileName)
        {
            Project project = projectRegistry.GetProject(projectId);

            // construct the file name
            string fullFileName = String.Format(CultureInfo.InvariantCulture,
                "Storage{0}Projects{0}{1}{0}{2}",
                Path.DirectorySeparatorChar,
                projectId,
                localFileName);

            return fullFileName;
        }

        public string FetchFile(string projectId, string localFileName)
        {
            Project project = projectRegistry.GetProject(projectId);

            // construct the file name
            string fullFileName = GetFullFileName(projectId, localFileName);

            if (File.Exists(fullFileName))
                return File.ReadAllText(fullFileName);
            else
                return null;
        }

        private readonly IProjectRegistry projectRegistry;
    }
}