using ProjectPilot.Log4NetBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;


namespace ProjectPilot.Log4NetBrowser.Views.LogView
{
    public partial class DisplayLogFiles : ViewPage
    {
        public void GetLogFiles()
        {
            LogDisplay parserContent = new LogDisplay();

            Dictionary<string, string> logFiles = new Dictionary<string, string>();

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreProcessingInstructions = true;
            xmlReaderSettings.IgnoreWhitespace = true;

            using (XmlReader xmlReader = XmlReader.Create(System.IO.File.OpenRead(@"\\zarja\share\Marko\LogConfig.xml"), xmlReaderSettings))
            {
                string key;

                while (false == xmlReader.EOF)
                {
                    xmlReader.Read();

                    if (xmlReader.Name == "LogURL" && xmlReader.NodeType != XmlNodeType.EndElement)
                    {
                        key = xmlReader["logKey"];

                        xmlReader.Read();

                        logFiles.Add(key, xmlReader.Value);
                    }
                }
            }

            ViewData["LogFiles"] = logFiles;
        }
    }
}
        