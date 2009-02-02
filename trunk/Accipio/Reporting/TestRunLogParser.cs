using System;
using System.Globalization;
using System.IO;
using System.Xml;
using Accipio.Reporting;

namespace Accipio.Reporting
{
    public class TestRunLogParser
    {
        /// <summary>
        /// Initializes a new instance of the TestRunLogParser class.
        /// </summary>
        /// <param name="testRunLogFileName">The name of test run log file.</param>
        public TestRunLogParser(string testRunLogFileName)
        {
            this.testRunLogFileName = testRunLogFileName;
        }

        /// <summary>
        /// Parse xml data which contains build report data.
        /// </summary>
        /// <returns>Parsed data</returns>
        public TestRun Parse()
        {
            using (FileStream xmlReportStream = File.OpenRead(testRunLogFileName))
            {
                XmlReaderSettings xmlReaderSettings =
                    new XmlReaderSettings
                        {
                            IgnoreComments = true,
                            IgnoreProcessingInstructions = true,
                            IgnoreWhitespace = true
                        };

                TestRun testRun = new TestRun();

                using (XmlReader xmlReader = XmlReader.Create(xmlReportStream, xmlReaderSettings))
                {
                    xmlReader.Read();

                    while (false == xmlReader.EOF)
                    {
                        switch (xmlReader.NodeType)
                        {
                            case XmlNodeType.Element:
                                {
                                    if (xmlReader.Name != "report")
                                        throw new XmlException("<report> (root) element expected.");

                                    string accipioVersion = xmlReader.GetAttribute("accipioVersion");
                                    if (false == String.IsNullOrEmpty(accipioVersion))
                                        testRun.AccipioVersion = new Version(accipioVersion);

                                    string testedSoftwareVersion = xmlReader.GetAttribute("testedSoftwareVersion");
                                    if (false == String.IsNullOrEmpty(testedSoftwareVersion))
                                        testRun.TestedSoftwareVersion = new Version(testedSoftwareVersion);

                                    ReadTestRunParameters(testRun, xmlReader);

                                    break;
                                }

                            case XmlNodeType.XmlDeclaration:
                                xmlReader.Read();
                                continue;

                            default:
                                {
                                    throw new NotSupportedException(
                                        string.Format(
                                            CultureInfo.InvariantCulture,
                                            "Not supported xml node type. Node type = {0}",
                                            xmlReader.NodeType));
                                }
                        }
                    }
                }

                testRun.FillUserStoriesData();

                return testRun;
            }
        }

        private void ReadTestSuiteRun(TestRun testRun, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "suite":
                        string suiteId = ReadAttribute(xmlReader, "id");
                        TestSuiteRun testSuiteRun = new TestSuiteRun(suiteId);
                        ReadTestCaseRuns(testSuiteRun, xmlReader);
                        testRun.AddTestSuiteRun(testSuiteRun);

                        break;

                    default:
                        {
                            throw new NotSupportedException(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Not supported xml node type. Node type = {0}",
                                    xmlReader.NodeType));
                        }
                }
            }

            xmlReader.Read();
        }

        private void ReadTestCaseRuns(TestSuiteRun testSuiteRun, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "case":
                        string gallioStatus = ReadAttribute(xmlReader, "status");
                        TestExecutionStatus status = TestExecutionStatus.NotImplemented;

                        switch (gallioStatus)
                        {
                            case "passed":
                                status = TestExecutionStatus.Successful;
                                break;

                            case "failed":
                                status = TestExecutionStatus.Failed;
                                break;

                            case "pending":
                            case "skipped":
                            case "inconclusive":
                                status = TestExecutionStatus.NotImplemented;
                                break;

                            default:
                                throw new NotSupportedException(String.Format(CultureInfo.InvariantCulture, "Gallio test status '{0}' not supported", gallioStatus));
                        }

                        TestCaseRun testCaseRun = new TestCaseRun(
                            ReadAttribute(xmlReader, "id"),
                            status);

                        ReadUserStories(testCaseRun, xmlReader);

                        testSuiteRun.AddTestCaseRun(testCaseRun);

                        break;

                    default:
                        {
                            throw new NotSupportedException(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Not supported xml node type. Node type = {0}",
                                    xmlReader.NodeType));
                        }
                }
            }

            xmlReader.Read();
        }

        private void ReadTestRunParameters(TestRun reportData, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "testRun":
                        double duration = double.Parse(ReadAttribute(xmlReader, "duration"), CultureInfo.InvariantCulture);
                        reportData.StartTime = Convert.ToDateTime(ReadAttribute(xmlReader, "startTime"), CultureInfo.InvariantCulture);
                        reportData.EndTime = reportData.StartTime.AddSeconds(duration);
                        // TODO:
                        //reportData.Version = new Version(ReadAttribute(xmlReader, "version"));

                        ReadSuites(reportData, xmlReader);

                        break;
                        
                    default:
                        {
                            throw new NotSupportedException(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Not supported xml node type. Node type = {0}",
                                    xmlReader.NodeType));
                        }
                }
            }

            xmlReader.Read();
        }

        private void ReadSuites(TestRun reportData, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "suites":

                        ReadTestSuiteRun(reportData, xmlReader);

                        break;

                    default:
                        {
                            throw new NotSupportedException(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Not supported xml node type. Node type = {0}",
                                    xmlReader.NodeType));
                        }
                }
            }

            xmlReader.Read();
        }

        private void ReadUserStories(TestCaseRun testCaseRun, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "userStories":

                        ReadUserStory(testCaseRun, xmlReader);

                        break;

                    default:
                        {
                            throw new NotSupportedException(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Not supported xml node type. Node type = {0}",
                                    xmlReader.NodeType));
                        }
                }
            }

            xmlReader.Read();
        }

        private void ReadUserStory(TestCaseRun testCaseRun, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "userStory":
                        testCaseRun.AddUserStory(xmlReader.ReadElementContentAsString());
                        break;

                    case "exception":
                        testCaseRun.Message = xmlReader.ReadElementContentAsString();
                        break;

                    default:
                        {
                            throw new NotSupportedException(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Not supported xml node type. Node type = {0}",
                                    xmlReader.NodeType));
                        }
                }
            }

            xmlReader.Read();
        }

        private static string ReadAttribute(XmlReader xmlReader, string attributeName)
        {
            return xmlReader.GetAttribute(attributeName);
        }

        private readonly string testRunLogFileName;
    }
}