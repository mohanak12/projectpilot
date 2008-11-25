using System;
using System.IO;

namespace Accipio
{
    public class FileCodeWriter : ICodeWriter
    {
        public FileCodeWriter(string fileName)
        {
            writer = new StreamWriter(fileName);
        }

        public void Close()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }

        public void WriteLine(string line)
        {
            writer.WriteLine(line);
        }

        public void WriteLine(string format, params object[] args)
        {
            writer.WriteLine(format, args);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <code>false</code>, cleans up native resources. 
        /// If <code>true</code> cleans up both managed and native resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                    Close();
                }

                disposed = true;
            }
        }

        private bool disposed;

        #endregion

        private StreamWriter writer;
    }
}