using System;
using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework
{
    public interface IFileManager
    {
        string CreateNewFile(string directory, string localFileName);

        void DeleteFile(string fileName);

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        T DeserializeFromFile<T>(string fileName);
        
        void EnsurePathExists(string path);
        
        string GetFullFileName(string domain, string localFileName);
        
        string GetProjectFullFileName(string projectId, string moduleId, string localFileName, bool ensureDirPathExists);
        
        string FetchProjectFile(string projectId, string moduleId, string localFileName);
        
        bool FileExists(string fileName);

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        void SerializeIntoFile<T>(string fileName, T value);

        /// <summary>
        /// Translates the relative file name of a file within ProjectPilot Web application's root to an URL through
        /// which this file can be accessed via Web.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>An URL through which this file can be accessed via Web</returns>
        Uri TranslateToUrl(string fileName);
    }
}