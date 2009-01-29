using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Accipio
{
    /// <summary>
    /// Contains various helper methods used in Accipio.
    /// </summary>
    public sealed class AccipioHelper
    {
        public static void EnsureDirectoryPathExists(string path, bool containsFileName)
        {
            // remove the file name if it is a part of the path
            if (containsFileName)
            {
                EnsureDirectoryPathExists(Path.GetDirectoryName(path), false);
                return;
            }

            if (Directory.Exists(path))
                return;

            string parentPath = Path.GetDirectoryName(path);

            if (false == String.IsNullOrEmpty(parentPath) && false == Directory.Exists(parentPath))
                EnsureDirectoryPathExists(parentPath, false);

            Directory.CreateDirectory(path);
        }

        private AccipioHelper()
        {
        }
    }
}
