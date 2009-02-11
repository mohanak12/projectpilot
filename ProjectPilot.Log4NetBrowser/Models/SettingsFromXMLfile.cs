using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.SessionState;

namespace ProjectPilot.Log4NetBrowser.Models
{
    public class SettingsFromXMLfile
    {
        public static Dictionary<string, string> Read(string settingsXMLfilePath)
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();

            Dictionary<string, string> logFiles = new Dictionary<string, string>();
            Dictionary<string, string> logFilesPatterns = new Dictionary<string, string>();
            Dictionary<string, char> logFilesSeparators = new Dictionary<string, char>();

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreProcessingInstructions = true;
            xmlReaderSettings.IgnoreWhitespace = true;

            using (XmlReader xmlReader = XmlReader.Create(System.IO.File.OpenRead(settingsXMLfilePath), xmlReaderSettings))
            
            {
                while (false == xmlReader.EOF)
                {
                    xmlReader.Read();

                    if (xmlReader.Name == "FilterNumberOfLogItems" && xmlReader.NodeType != XmlNodeType.EndElement)
                    {
                        xmlReader.Read();
                        settings.Add("FilterNumberOfLogItems", xmlReader.Value);
                    }

                    if (xmlReader.Name == "NumberOfItemsPerPage" && xmlReader.NodeType != XmlNodeType.EndElement)
                    {
                        xmlReader.Read();
                        settings.Add("NumberOfItemsPerPage", xmlReader.Value);
                    }
                }
            }

            return settings;
       }
    }
}
