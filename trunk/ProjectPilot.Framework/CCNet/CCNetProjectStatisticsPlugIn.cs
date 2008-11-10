using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;

namespace ProjectPilot.Framework.CCNet
{
    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public class CCNetProjectStatisticsPlugIn
    {
        #region Public method

        static public CCNetProjectStatisticsData Load(Stream stream)
        {
            CCNetProjectStatisticsData data = new CCNetProjectStatisticsData();

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreProcessingInstructions = true;
            xmlReaderSettings.IgnoreWhitespace = true;

            using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
            {
                xmlReader.Read();

                while (false == xmlReader.EOF)
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.XmlDeclaration:
                            xmlReader.Read();
                            continue;

                        case XmlNodeType.Element:
                            if (xmlReader.Name != "statistics")
                                throw new XmlException("<statistics> (root) element expected.");

                            ReadStatistics(data, xmlReader);

                            return data;

                        default:
                            throw new XmlException();
                    }
                }
            }

            return null;
        }

        #endregion

        #region Private methods

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "xmlReader")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "data")]
        private static void ReadStatistics(CCNetProjectStatisticsData data, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "timestamp":

                        xmlReader.Read();

                        break;

                    case "integration":

                        CCNetProjectStatisticsBuildEntry entry = new CCNetProjectStatisticsBuildEntry();
                        entry.BuildLabel = ReadAttribute(xmlReader, "build-label");
                        entry.BuildStatus = ReadAttribute(xmlReader, "status");

                        ReadIntegrationEntry(entry, xmlReader);

                        data.Builds.Add(entry);

                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        private static void ReadIntegrationEntry(CCNetProjectStatisticsBuildEntry data, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "statistic":

                        string key = ReadAttribute(xmlReader, "name");
                        string value = xmlReader.ReadElementContentAsString();

                        data.Parameters.Add(key, value);

                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            xmlReader.Read();
        }

        private static string ReadAttribute(XmlReader xmlReader, string attributeName)
        {
            try
            {
                return xmlReader.GetAttribute(attributeName);
            }
            catch (Exception)
            {
                throw new NotSupportedException();
            }
        }

        #endregion
    }
}
