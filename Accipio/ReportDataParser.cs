using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Accipio
{
    public class ReportDataParser : IDisposable
    {
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

        private void ReadTestRunParameters(ReportData reportData, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "testRun":

                        reportData.Duration = ReadAttribute(xmlReader, "startTime");
                        reportData.StartTime = ReadAttribute(xmlReader, "duration");
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

                        ReportCase reportCase = new ReportCase(
                            ReadAttribute(xmlReader, "id"),
                            ReadAttribute(xmlReader, "startTime"),
                            ReadAttribute(xmlReader, "duration"),
                            (ReportCaseStatus)Enum.Parse(typeof(ReportCaseStatus), ReadAttribute(xmlReader, "status"), true));

                        ReadUserStories(reportCase, xmlReader);

                        reportSuite.TestCases.Add(reportCase);

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

        private void ReadUserStories(ReportCase reportCase, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "userStories":

                        ReadUserStory(reportCase, xmlReader);

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

        private void ReadUserStory(ReportCase reportCase, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "userStory":

                        reportCase.UserStories.Add(xmlReader.ReadElementContentAsString());

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
