using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Flubu.Tasks.FileSystem
{
    public class ZipFilesTask : TaskBase
    {
        public ZipFilesTask (string zipFileName, string baseDir, IEnumerable<string> filesToZip)
        {
            this.zipFileName = zipFileName;
            this.baseDir = baseDir;
            this.filesToZip.AddRange (filesToZip);

            Path.GetExtension (zipFileName);
        }

        public override string TaskDescription
        {
            get
            {
                return String.Format (
                    CultureInfo.InvariantCulture,
                    "Zipping {1} files to the '{0}' archive, using the base directory '{2}'",
                    zipFileName,
                    filesToZip.Count,
                    baseDir);
            }
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            using (FileStream zipFileStream = new FileStream (
                zipFileName,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.None))
            {
                using (ZipOutputStream zipStream = new ZipOutputStream (zipFileStream))
                {
                    byte[] buffer = new byte[1024 * 1024];

                    foreach (string fileName in filesToZip)
                    {
                        int skipChar = 0;

                        if (false == String.IsNullOrEmpty (baseDir)
                            && (baseDir[baseDir.Length - 1] == '\\'
                            || baseDir[baseDir.Length - 1] == '/'))
                            skipChar++;

                        // cut off the leading part of the path (up to the root directory of the package)
                        string basedFileName = fileName.Substring (baseDir.Length + skipChar);

                        basedFileName = ZipEntry.CleanName (basedFileName);

                        environment.LogMessage ("Zipping file '{0}'", basedFileName);

                        using (FileStream fileStream = File.OpenRead (fileName))
                        {
                            ZipEntry entry = new ZipEntry (basedFileName);
                            entry.DateTime = File.GetLastWriteTime (fileName);
                            entry.Size = fileStream.Length;
                            zipStream.PutNextEntry (entry);

                            int sourceBytes;

                            while (true)
                            {
                                sourceBytes = fileStream.Read (buffer, 0, buffer.Length);

                                if (sourceBytes == 0)
                                    break;

                                zipStream.Write (buffer, 0, sourceBytes);
                            }
                        }
                    }
                }
            }
        }

        private readonly string baseDir;
        private List<string> filesToZip = new List<string> ();
        private readonly string zipFileName;
    }
}