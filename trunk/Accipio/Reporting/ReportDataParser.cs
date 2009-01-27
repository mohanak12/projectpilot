using System;
using System.Globalization;
using System.IO;
using System.Xml;
using Accipio.Reporting;

namespace Accipio.Reporting
{
    public class ReportDataParser : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the ReportDataParser class.
        /// </summary>
        /// <param name="testSuiteFileName">File name of report.</param>
        public ReportDataParser(string testSuiteFileName)
        {
            xmlReportStream = File.OpenRead(testSuiteFileName);
        }

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
        /// Parse xml data which contains build report data.
        /// </summary>
        /// <returns>Parsed data</returns>
        public ReportData Parse()
        {
            XmlReaderSettings xmlReaderSettings =
                new XmlReaderSettings
                    {
                        IgnoreComments = true,
                        IgnoreProcessingInstructions = true,
                        IgnoreWhitespace = true
                    };

            ReportData reportData = new ReportData();

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

                                ReadTestRunParameters(reportData, xmlReader);

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

            return reportData;
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
                    xmlReportStream.Dispose();
                }

                disposed = true;
            }
        }

        private void ReadReportSuite(ReportData reportData, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "suite":

                        string reportSuiteId = ReadAttribute(xmlReader, "id");

                        ReportSuite reportSuite = new ReportSuite(reportSuiteId);

                        ReadReportCases(reportSuite, xmlReader);

                        reportData.TestSuites.Add(reportSuite);

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

        private void ReadReportCases(ReportSuite reportSuite, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "case":

                        TestCaseExecutionStatus caseExecutionStatus =
                            (TestCaseExecutionStatus)Enum.Parse(typeof(TestCaseExecutionStatus), ReadAttribute(xmlReader, "status"), true);

                        TestCaseExecutionReport testCaseExecutionReport = new TestCaseExecutionReport(
                            ReadAttribute(xmlReader, "id"),
                            Convert.ToDateTime(ReadAttribute(xmlReader, "startTime"), CultureInfo.InvariantCulture),
                            ReadAttribute(xmlReader, "duration"),
                            caseExecutionStatus);

                        if (caseExecutionStatus == TestCaseExecutionStatus.Failed)
                            reportSuite.FailedTests++;
                        else if (caseExecutionStatus == TestCaseExecutionStatus.Passed)
                            reportSuite.PassedTests++;
                        else if (caseExecutionStatus == TestCaseExecutionStatus.Skipped)
                            reportSuite.SkippedTests++;

                        ReadUserStories(testCaseExecutionReport, xmlReader);

                        reportSuite.TestCases.Add(testCaseExecutionReport);

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

        private void ReadTestRunParameters(ReportData reportData, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "testRun":

                        reportData.Duration = ReadAttribute(xmlReader, "duration");
                        reportData.StartTime = Convert.ToDateTime(ReadAttribute(xmlReader, "startTime"), CultureInfo.InvariantCulture);
                        reportData.Version = ReadAttribute(xmlReader, "version");

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

        private void ReadSuites(ReportData reportData, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "suites":

                        ReadReportSuite(reportData, xmlReader);

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

        private void ReadUserStories(TestCaseExecutionReport testCaseExecutionReport, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "userStories":

                        ReadUserStory(testCaseExecutionReport, xmlReader);

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

        private void ReadUserStory(TestCaseExecutionReport testCaseExecutionReport, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "userStory":

                        testCaseExecutionReport.UserStories.Add(xmlReader.ReadElementContentAsString());

                        break;

                    case "exception":

                        testCaseExecutionReport.ReportDetails = xmlReader.ReadElementContentAsString();

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

        private bool disposed;
        private readonly Stream xmlReportStream;
    }
}