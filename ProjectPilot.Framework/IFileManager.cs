using System;

namespace ProjectPilot.Framework
{
    public interface IFileManager
    {
        string GetFullFileName(string domain, string localFileName);
        string GetProjectFullFileName(string projectId, string moduleId, string localFileName, bool assertDirPathExists);
        string FetchProjectFile(string projectId, string moduleId, string localFileName);
        /// <summary>
        /// Translates the relative file name of a file within ProjectPilot Web application's root to an URL through
        /// which this file can be accessed via Web.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>An URL through which this file can be accessed via Web</returns>
        Uri TranslateToUrl(string fileName);
    }
}