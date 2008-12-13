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
            this.filesToZip.AddRange(filesToZip);

            Path.GetExtension(zipFileName);
        }

        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    CultureInfo.InvariantCulture,
                    "Zipping {1} files to the '{0}' archive",
                    zipFileName,
                    filesToZip.Count);
            }
        }

        protected override void DoExecute(IScriptExecutionEnvironment environment)
        {
            using (FileStream zipFileStream = new FileStream(
                zipFileName,
                FileMode.Create,
                FileAccess.ReadWrite,
                FileShare.None))
            {
                using (ZipOutputStream zipStream = new ZipOutputStream(zipFileStream))
                {
                    byte[] buffer = new byte[1024 * 1024];

                    foreach (string fileName in filesToZip)
                    {
                        // cut off the leading part of the path (up to the root directory of the package)
                        string basedFileName = fileName.Substring(baseDir.Length + 1);

                        environment.LogMessage("Zipping file '{0}'", basedFileName);

                        ZipEntry entry = new ZipEntry(basedFileName);
                        entry.DateTime = File.GetLastWriteTime(fileName);
                        zipStream.PutNextEntry(entry);

                        using (FileStream fileStream = File.OpenRead(fileName))
                        {
                            int sourceBytes;

                            do
                            {
                                sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                                zipStream.Write(buffer, 0, sourceBytes);
                            } 
                            while (sourceBytes > 0);
                        }
                    }
                }
            }
        }

        private readonly string baseDir;
        private List<string> filesToZip = new List<string>();
        private readonly string zipFileName;
    }
}