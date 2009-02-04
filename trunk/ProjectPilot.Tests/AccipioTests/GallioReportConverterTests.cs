using System.IO;
using System.Xml;
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

        [Test, Pending("TODO: Igor")] 
        public void MultilineErrorMessage()
        {
            if (Directory.Exists("TestLogs"))
                Directory.Delete("TestLogs", true);

            string[] args = new string[]
                                {
                                    @"-i=..\..\AccipioTests\Samples\GallioTestResults1.xml",
                                    "-o=TestLogs"
                                };
            GallioReportConverter gallioReportConverter = new GallioReportConverter();
            Assert.AreEqual(0, gallioReportConverter.Execute(args));

            string[] files = Directory.GetFiles("TestLogs");
            Assert.IsTrue(files.Length > 0);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(files[0]);
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            xmlNamespaceManager.AddNamespace("a", "http://projectpilot/AccipioTestRunReport.xsd");

            XmlNode node = xmlDoc.SelectSingleNode(
                "/a:report/a:testRun/a:suites/a:suite/a:case[@id='RemoveAllMmsSubsFromSpGui']/a:error",
                xmlNamespaceManager);

            Assert.IsNotNull(node);
            Assert.IsFalse(string.IsNullOrEmpty(node.Value));
        }
    }
}