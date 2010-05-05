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
        public void Attachment()
        {
            if (Directory.Exists("TestLogs"))
                Directory.Delete("TestLogs", true);

            string[] args = new string[]
                                {
                                    @"-i=..\..\AccipioTests\Samples\LastTestResults.xml",
                                    "-o=TestLogs"
                                };
            ReportConverter reportConverter = new ReportConverter();
            Assert.AreEqual(0, reportConverter.Execute(args));

            string[] files = Directory.GetFiles("TestLogs");
            Assert.IsTrue(files.Length > 0);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(files[0]);
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            xmlNamespaceManager.AddNamespace("a", "http://projectpilot/AccipioTestRunReport.xsd");

            XmlNode node = xmlDoc.SelectSingleNode(
                "/a:report/a:testRun/a:suites/a:suite/a:case[@id='Failed']/a:attachments/a:attachment",
                xmlNamespaceManager);
            
            Assert.IsNotNull(node);
            Assert.AreEqual("Failed test", node.Attributes["name"].Value, "Name attribute");
            
            node = xmlDoc.SelectSingleNode(
                "/a:report/a:testRun/a:suites/a:suite/a:case[@id='Failed1']/a:attachments/a:attachment",
                xmlNamespaceManager);

            Assert.IsNotNull(node);
            Assert.AreEqual(@"LastTestResults\b689dd6e6cc01622\Failed1 test.png", node.Attributes["contentPath"].Value, "cantentPath attribute");
        }

        [Test]
        public void TransformGallioTestResults()
        {
            string[] args = new string[]
                                {
                                    "-i=..\\..\\..\\Data\\Samples\\AcceptanceTestResults.xml",
                                    "-o=TestLogs"
                                };
            ReportConverter reportConverter = new ReportConverter();
            Assert.AreEqual(0, reportConverter.Execute(args));

            HtmlTestReportGeneratorCommand cmd = new HtmlTestReportGeneratorCommand();
            args = new string[]
                       {
                           "-i=TestLogs"
                       };
            Assert.AreEqual(0, cmd.Execute(args));
        }

        [Test]
        public void TransformNUnitTestResults()
        {
            string[] args = new string[]
                                {
                                    "-i=..\\..\\..\\Data\\Samples\\Hsl.SsmTest.SystemTest.dll-results.xml",
                                    "-o=TestLogs",
                                    "-x=TestReportTransform.NUnit.xslt",
                                    "-t=NUnit"
                                };
            ReportConverter reportConverter = new ReportConverter();
            Assert.AreEqual(0, reportConverter.Execute(args));

            HtmlTestReportGeneratorCommand cmd = new HtmlTestReportGeneratorCommand();
            args = new string[]
                       {
                           "-i=TestLogs"
                       };
            Assert.AreEqual(0, cmd.Execute(args));
        }

        [Test] 
        public void MultilineErrorMessage()
        {
            if (Directory.Exists("TestLogs"))
                Directory.Delete("TestLogs", true);

            string[] args = new string[]
                                {
                                    @"-i=..\..\AccipioTests\Samples\GallioTestResults1.xml",
                                    "-o=TestLogs"
                                };
            ReportConverter reportConverter = new ReportConverter();
            Assert.AreEqual(0, reportConverter.Execute(args));

            string[] files = Directory.GetFiles("TestLogs");
            Assert.IsTrue(files.Length > 0);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(files[0]);
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            xmlNamespaceManager.AddNamespace("a", "http://projectpilot/AccipioTestRunReport.xsd");

            XmlNode node = xmlDoc.SelectSingleNode(
                "/a:report/a:testRun/a:suites/a:suite/a:case[@id='RemoveAllMmsSubsFromSpGui']/a:message",
                xmlNamespaceManager);

            Assert.IsNotNull(node);
            Assert.IsFalse(string.IsNullOrEmpty(node.InnerText));
        }

        /// <summary>
        /// Makes sure the warning messages are also extracted from the Gallio report.
        /// </summary>
        [Test]
        public void WarningMessage()
        {
            if (Directory.Exists("TestLogs"))
                Directory.Delete("TestLogs", true);

            string[] args = new string[]
                                {
                                    @"-i=..\..\AccipioTests\Samples\GallioTestResults1.xml",
                                    "-o=TestLogs"
                                };
            ReportConverter reportConverter = new ReportConverter();
            Assert.AreEqual(0, reportConverter.Execute(args));

            string[] files = Directory.GetFiles("TestLogs");
            Assert.IsTrue(files.Length > 0);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(files[0]);
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            xmlNamespaceManager.AddNamespace("a", "http://projectpilot/AccipioTestRunReport.xsd");

            XmlNode node = xmlDoc.SelectSingleNode(
                "/a:report/a:testRun/a:suites/a:suite/a:case[@id='AddDifferentSubsFromTopTopicLinkMms']/a:message",
                xmlNamespaceManager);

            Assert.IsNotNull(node);
            Assert.IsFalse(string.IsNullOrEmpty(node.InnerText));
        }
    }
}