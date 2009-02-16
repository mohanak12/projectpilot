using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

namespace ProjectPilot.Log4NetBrowser.Models
{
    public class ParseLogFile
    {
        public static LogDisplay ParseFile(string logKey, string settingsXMLfilePath)
        {
            LogDisplay parserContent = new LogDisplay();

            Dictionary<string, string> logFiles = new Dictionary<string, string>();
            Dictionary<string, string> logFilesPatterns = new Dictionary<string, string>();
            Dictionary<string, char> logFilesSeparators = new Dictionary<string, char>();

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreProcessingInstructions = true;
            xmlReaderSettings.IgnoreWhitespace = true;

            using (XmlReader xmlReader = XmlReader.Create(System.IO.File.OpenRead(settingsXMLfilePath), xmlReaderSettings))
            {
                string key;

                while (false == xmlReader.EOF)
                {
                    xmlReader.Read();

                    if (xmlReader.Name == "LogURL" && xmlReader.NodeType != XmlNodeType.EndElement)
                    {
                        key = xmlReader["logKey"];

                        logFilesPatterns.Add(key, xmlReader["logPattern"]);
                        logFilesSeparators.Add(key, char.Parse(xmlReader["separator"]));

                        xmlReader.Read();

                        logFiles.Add(key, xmlReader.Value);
                    }
                }
            }

            string fileSelected = string.Empty;
            string filePattern = string.Empty;
            char fileSeparator = '|'; //Default separator

            if (logFiles.ContainsKey(logKey))
            {
                fileSelected = logFiles[logKey];
                filePattern = logFilesPatterns[logKey];
                fileSeparator = logFilesSeparators[logKey];
                
                parserContent.Parsing10MBLogFile(null, fileSelected, filePattern, fileSeparator);
            }
            else
            {
                parserContent.Parsing10MBLogFile(null, null, null, null);
            }
            
            return parserContent;
        }

        public static string GetLogId(string logPath, string settingsXMLfilePath)
        {
            LogDisplay parserContent = new LogDisplay();
            string id;

            Dictionary<string, string> logFiles = new Dictionary<string, string>();
            Dictionary<string, string> logFilesPatterns = new Dictionary<string, string>();
            Dictionary<string, char> logFilesSeparators = new Dictionary<string, char>();

            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreProcessingInstructions = true;
            xmlReaderSettings.IgnoreWhitespace = true;

            using (XmlReader xmlReader = XmlReader.Create(System.IO.File.OpenRead(settingsXMLfilePath), xmlReaderSettings))
            {
                string key;

                while (false == xmlReader.EOF)
                {
                    xmlReader.Read();

                    if (xmlReader.Name == "LogURL" && xmlReader.NodeType != XmlNodeType.EndElement)
                    {
                        key = xmlReader["logKey"];

                        logFilesPatterns.Add(key, xmlReader["logPattern"]);
                        logFilesSeparators.Add(key, char.Parse(xmlReader["separator"]));

                        xmlReader.Read();

                        logFiles.Add(key, xmlReader.Value);
                    }
                }
            }

            int count = 0;
            int keyIndex = 0;

            foreach (string path in logFiles.Values)
            {
                if (path == logPath)
                {
                    keyIndex = count;
                }
                count++;
            }

            id = logFiles.ElementAt(keyIndex).Key;

            return id;
        }
    }
}
