using System.Collections.Generic;
using System.IO;

namespace Flubu.Packaging
{
    public class DirectoryFilesLister : IDirectoryFilesLister
    {
        public IEnumerable<string> ListFiles(string directoryName)
        {
            List<string> files = new List<string>();
            ListFilesPrivate(directoryName, files);
            return files;
        }

        private static void ListFilesPrivate(string directoryName, List<string> files)
        {
            foreach (string file in Directory.GetFiles(directoryName))
                files.Add(file);

            foreach (string directory in Directory.GetDirectories(directoryName))
                ListFilesPrivate(directory, files);
        }
    }
}