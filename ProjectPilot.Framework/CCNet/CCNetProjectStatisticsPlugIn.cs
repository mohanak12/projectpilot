using System;
using System.IO;
using System.Xml;

namespace ProjectPilot.Framework.CCNet
{
    /// <summary>
    /// Implementation of <see cref="ICCNetProjectStatisticsPlugIn" /> interface to
    /// fetch ccnet statistics data and parse data to object <see cref="ProjectStatsData"/>.
    /// </summary>
    public class CCNetProjectStatisticsPlugIn : ICCNetProjectStatisticsPlugIn
    {
        /// <summary>
        /// Fetch ccnet statistics from server where cruise control is running
        /// </summary>
        /// <returns>Returns object <see cref="ProjectStatsData"/> of ccnet statistics data</returns>
        public ProjectStatsData FetchStatistics()
        {
            ProjectStatsData data;

            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                data = Load(stream);
            }

            return data;
        }

        public static ProjectStatsData Load(Stream stream)
        {
            ProjectStatsData data = new ProjectStatsData();

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
                        case XmlNodeType.Element:
                            if (xmlReader.Name != "statistics")
                                throw new XmlException("<statistics> (root) element expected.");

                            ReadStatistics(data, xmlReader);

                            break;
                            
                        case XmlNodeType.XmlDeclaration:
                            xmlReader.Read();
                            continue;

                        default:
                            throw new XmlException();
                    }
                }
            }

            return data;
        }

        private static void AddBuildStatus(ProjectStatsBuildEntry data, string status)
        {
            //Add build status parameters to parameter list
            data.Parameters.Add("Success", "0");
            data.Parameters.Add("Exception", "0");
            data.Parameters.Add("Failure", "0");

            switch (status)
            {
                case "Exception":
                    data.Parameters["Exception"] = "1";
                    break;
                case "Failure":
                    data.Parameters["Failure"] = "1";
                    break;
                case "Success":
                    data.Parameters["Success"] = "1";
                    break;
            }
        }

        private static void ReadStatistics(ProjectStatsData data, XmlReader xmlReader)
        {
            xmlReader.Read();

            int buildIdCounter = 0;
            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "timestamp":

                        xmlReader.Read();

                        break;

                    case "integration":

                        ProjectStatsBuildEntry entry = new ProjectStatsBuildEntry(buildIdCounter++);
                        entry.BuildLabel = ReadAttribute(xmlReader, "build-label");

                        ReadIntegrationEntry(entry, ReadAttribute(xmlReader, "status"), xmlReader);

                        data.Builds.Add(entry);

                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            xmlReader.Read();
        }

        private static void ReadIntegrationEntry(ProjectStatsBuildEntry data, string status, XmlReader xmlReader)
        {
            xmlReader.Read();

            AddBuildStatus(data, status);

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
            return xmlReader.GetAttribute(attributeName);
        }
    }
}
