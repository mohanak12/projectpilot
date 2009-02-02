using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class GallioReportConverterTests
    {
        [Test]
        public void TransformGallioTestResults()
        {
            string[] args = new string[]
                                {
                                    "-i=..\\..\\..\\Data\\Samples\\AcceptanceTestResults.xml",
                                    "-o=TestLogs"
                                };
            GallioReportConverter gallioReportConverter = new GallioReportConverter();
            Assert.AreEqual(0, gallioReportConverter.Execute(args));

            HtmlTestReportGeneratorCommand cmd = new HtmlTestReportGeneratorCommand();
            args = new string[]
                       {
                           "-i=TestLogs"
                       };
            Assert.AreEqual(0, cmd.Execute(args));
        }
    }
}