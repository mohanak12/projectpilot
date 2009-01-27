using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class TestReportGeneratorCommandTests
    {
        [Test]
        public void TransformGallioTestResults()
        {
            string[] args = new string[]
                                {
                                    "-i=..\\..\\..\\Data\\Samples\\AcceptanceTestResults.xml"
                                };
            GallioReportConverter gallioReportConverter = new GallioReportConverter();
            Assert.AreEqual(0, gallioReportConverter.Execute(args));
        }
    }
}