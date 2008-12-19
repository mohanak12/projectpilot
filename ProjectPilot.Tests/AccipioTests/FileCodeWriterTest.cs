using Accipio;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Unit tests for class <see cref="FileCodeWriter"/>
    /// </summary>
    [TestFixture]
    public class FileCodeWriterTest
    {
        /// <summary>
        /// Test writes text to file
        /// </summary>
        [Test]
        public void CodeWriterTest()
        {
            string fileName = @"file.txt";
            using (ICodeWriter codeWriter = new FileCodeWriter(fileName))
            {
                codeWriter.WriteLine("line");
            }

            FileCodeWriter fileCodeWriter = new FileCodeWriter(fileName);
            fileCodeWriter.WriteLine("file", new object[0]);
            fileCodeWriter.Close();
        }
    }
}
