using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProjectPilot.Framework.CCNet
{
    [SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public class CCNetProjectStatisticsPlugIn
    {
        static public CCNetProjectStatisticsData Load (Stream stream)
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
                            if (xmlReader.Name != "log")
                                throw new XmlException("<log> element expected.");

                            ReadStatistics(data, xmlReader);
                            return data;

                        default:
                            throw new XmlException();
                    }
                }
            }

            return null;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "reader")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "data")]
        private static void ReadStatistics(object data, XmlReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
