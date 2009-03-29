using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public ZipFileCallback ZipFileFooterCallback
        {
            get { return zipFileFooterCallback; }
            set { zipFileFooterCallback = value; }
        }

        public ZipFileCallback ZipFileHeaderCallback
        {
            get { return zipFileHeaderCallback; }
            set { zipFileHeaderCallback = value; }
        }

        [SuppressMessage ("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate void ZipFileCallback (string fileName, ZipOutputStream zipOutputStream);

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
                    buffer = new byte[1024 * 1024];

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
                        AddFileToZip(fileName, basedFileName, zipStream);
                    }
                }
            }
        }

        private void AddFileToZip(string fileName, string basedFileName, ZipOutputStream zipStream)
        {
            using (FileStream fileStream = File.OpenRead (fileName))
            {
                ZipEntry entry = new ZipEntry (basedFileName);
                entry.DateTime = File.GetLastWriteTime (fileName);
                entry.Size = fileStream.Length;
                zipStream.PutNextEntry (entry);

                if (zipFileHeaderCallback != null)
                    zipFileHeaderCallback(fileName, zipStream);

                int sourceBytes;

                while (true)
                {
                    sourceBytes = fileStream.Read (buffer, 0, buffer.Length);

                    if (sourceBytes == 0)
                        break;

                    zipStream.Write (buffer, 0, sourceBytes);
                }

                if (zipFileFooterCallback != null)
                    zipFileFooterCallback (fileName, zipStream);
            }
        }

        private readonly string baseDir;
        private byte[] buffer;
        private List<string> filesToZip = new List<string> ();
        private ZipFileCallback zipFileFooterCallback;
        private ZipFileCallback zipFileHeaderCallback;
        private readonly string zipFileName;
    }
}