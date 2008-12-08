using Accipio;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class FileCodeWriterTest
    {
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
