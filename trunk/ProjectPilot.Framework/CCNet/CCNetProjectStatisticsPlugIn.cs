using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;

namespace ProjectPilot.Framework.CCNet
{
    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public class CCNetProjectStatisticsPlugIn : ICCNetProjectStatisticsPlugIn
    {
        //#region Ctor

        //public CCNetProjectStatisticsPlugIn()
        //{}

        //public CCNetProjectStatisticsPlugIn(Uri url, string projectName)
        //{
        //    this.url = url;
        //    this.projectName = projectName;
        //}

        //#endregion

        #region Public method

        public CCNetProjectStatisticsData FetchStatistics()
        {
            CCNetProjectStatisticsData data;

            //Replace with correct value
            //RemoteCruiseManagerFactory factory = new RemoteCruiseManagerFactory();
            //Uri url = new Uri(string.Format(CultureInfo.InvariantCulture, "tcp://firefly:21234/CruiseManager.rem"));
            //string projectName = "ProjectPilot";
            //ICruiseManager mgr = factory.GetCruiseManager(url.ToString());

            //string proj = mgr.GetProject(projectName);
            //string stat = mgr.GetStatisticsDocument(projectName);

            using (Stream stream = File.OpenRead(@"..\..\..\Data\Samples\ccnet.stats.xml"))
            {
                data = Load(stream);
            }

            return data;
        }

        public static CCNetProjectStatisticsData Load(Stream stream)
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

        private static void AddBuildStatus(CCNetProjectStatisticsBuildEntry data, string status)
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

                        ReadIntegrationEntry(entry, ReadAttribute(xmlReader, "status"), xmlReader);

                        data.Builds.Add(entry);

                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        private static void ReadIntegrationEntry(CCNetProjectStatisticsBuildEntry data, string status, XmlReader xmlReader)
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

        //#region Private members


        //[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        //private Uri url;

        //[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        //private string projectName;

        //#endregion
    }
}
