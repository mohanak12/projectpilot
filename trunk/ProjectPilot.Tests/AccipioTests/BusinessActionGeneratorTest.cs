using System.IO;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class BusinessActionGeneratorTest
    {
        [Test]
        public void GetXmlFileContentTest()
        {
            Stream stream = AccipioHelper.GetXmlFileContent(@"..\..\..\Data\Samples\AccipioActions.xml");

            Assert.IsNotNull(stream);
        }

        [Test]
        public void ParseTest()
        {
            string fileName = @"..\..\..\Data\Samples\AccipioActions.xml";

            IGenerator generator = new BusinessActionGenerator();
            generator.Parse(new string[] { fileName });
        }
    }
}
