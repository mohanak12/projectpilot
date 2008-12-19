using System;
using System.Collections.Generic;
using System.Xml;

namespace ProjectPilot.Framework.Metrics
{
    /// <summary>
    /// Abstract class derived from LocMetricsBase. 
    /// Used to hold groups (nodes) of loc metrics items
    /// </summary>
    public abstract class GroupLocMetricsBase : LocMetricsBase
    {
        public IList<LocMetricsBase> GroupLocStatsData
        {
            get { return groupLocStatsData; }
        }

        /// <summary>
        /// Gets the loc stats data.
        /// </summary>
        /// <returns>Returns the combined loc stats of the group</returns>
        public override LocStatsData GetLocStatsData()
        {
            LocStatsData groupData = new LocStatsData(0, 0, 0);

            foreach (LocMetricsBase childLocMetrics in groupLocStatsData)
            {
                LocStatsData childLocStatsData = childLocMetrics.GetLocStatsData();
                groupData.Add(childLocStatsData);
            }

            return groupData;
        }

//        public override XmlNode InsertItem(XmlNode xmlNode, XmlDocument xmlDocument)
//        {
//            XmlNode item;
//            LocStatsData locStatsDataXML;
//            
//            foreach(LocStatsData locStatsData in this.groupLocStatsData)
//            {
//                locStatsDataXML.Cloc += locStatsData.Cloc;
//                locStatsDataXML.Sloc += locStatsData.Sloc;
//                locStatsDataXML.Eloc += locStatsData.Eloc;
//            }
//            
//            XmlElement element = xmlDocument.CreateElement("Item");
//            element.SetAttribute("FileName", this.filePath);
//            element.SetAttribute("FileType", "fileType");     //?
//
//            item = xmlNode = xmlNode.AppendChild(element);
//
//            element = xmlDocument.CreateElement("LoC");
//            xmlNode = xmlNode.AppendChild(element);
//            
//
//            element = xmlDocument.CreateElement("EmptyLinesOfCode");
//            element.InnerText = Convert.ToString(locStatsDataXML.Eloc);
//            xmlNode.AppendChild(element);
//
//            element = xmlDocument.CreateElement("SingleLinesOfCode");
//            element.InnerText = Convert.ToString(locStatsDataXML.Sloc);
//            xmlNode.AppendChild(element);
//
//            element = xmlDocument.CreateElement("CommentLinesOfCode");
//            element.InnerText = Convert.ToString(locStatsDataXML.Cloc);
//            xmlNode.AppendChild(element);
//
//            element = xmlDocument.CreateElement("SubItems");
//            xmlNode = item.AppendChild(element);
//
//            return xmlNode;
//        }

        protected GroupLocMetricsBase(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Adds another loc metrics item to teh group.
        /// </summary>
        /// <param name="locMetrics">Loc metrics item.</param>
        protected void AddLocMetrics(LocMetricsBase locMetrics)
        {
            groupLocStatsData.Add(locMetrics);
        }

        protected override void WriteSubitemsXml(XmlWriter writer)
        {
            writer.WriteStartElement("Subitem");

            foreach (LocMetricsBase childLocMetrics in groupLocStatsData)
            {
                childLocMetrics.WriteXml(writer);
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// The list of loc metrics items.
        /// </summary>
        private readonly List<LocMetricsBase> groupLocStatsData = new List<LocMetricsBase>();
    }
}
