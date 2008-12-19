using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace ProjectPilot.Framework.Metrics
{
    /// <summary>
    /// Abstract class for holding loc metrics objects.
    /// </summary>
    public abstract class LocMetricsBase
    {
        /// <summary>
        /// Gets the loc stats data.
        /// </summary>
        /// <returns>Returns loc metrics.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public abstract LocStatsData GetLocStatsData();

        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", MessageId = "System.Xml.XmlNode")]
        public XmlNode InsertItem(XmlNode xmlNode, XmlDocument xmlDocument)
        {
            XmlNode item;
            
            XmlElement element = xmlDocument.CreateElement("Item");
            element.SetAttribute("FileName", "filePath");
            element.SetAttribute("FileType", "fileType");
            
            item = xmlNode = xmlNode.AppendChild(element);
            
            element = xmlDocument.CreateElement("LoC");
            xmlNode = xmlNode.AppendChild(element);
            
            element = xmlDocument.CreateElement("EmptyLinesOfCode");
//            element.InnerText = Convert.ToString(this.GetLocStatsData().Eloc);
            element.InnerText = "0";
            xmlNode.AppendChild(element);
            
            element = xmlDocument.CreateElement("SingleLinesOfCode");
//            element.InnerText = Convert.ToString(this.GetLocStatsData().Sloc);
            element.InnerText = "0";
            xmlNode.AppendChild(element);
            
            element = xmlDocument.CreateElement("CommentLinesOfCode");
//            element.InnerText = Convert.ToString(this.GetLocStatsData().Cloc);
            element.InnerText = "0";
            xmlNode.AppendChild(element);
            
            element = xmlDocument.CreateElement("SubItems");
            xmlNode = item.AppendChild(element);
            
            return xmlNode;
        }
    }
}
