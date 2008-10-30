using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using ProjectPilot.Framework.RevisionControlHistory;

namespace ProjectPilot.Framework.Subversion
{
    public class SubversionHistoryPlugIn : IRevisionControlHistoryPlugIn
    {
        public SubversionHistoryPlugIn(Project project, string svnToolPath, string svnRootPath)
        {
            this.project = project;
            this.svnToolPath = svnToolPath;
            this.svnRootPath = svnRootPath;
        }

        public Project Project
        {
            get { return project; }
        }

        public string ModuleId
        {
            get { return "svn"; }
        }

        public string ModuleName
        {
            get { return "Subversion History"; }
        }

        public RevisionControlHistoryData FetchHistory()
        {
            // svn log d:\svn\mobilkom.nl-bhwr\trunk\src -v --xml --non-interactive >D:\BuildArea\builds\mobilkom.nl-bhwr\svn-log.xml
            using (Process process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = svnToolPath;
                process.StartInfo.Arguments = String.Format(CultureInfo.InvariantCulture, @"log ""{0}"" --xml --non-interactive",
                    svnRootPath);
                process.Start();

                process.WaitForExit();

                RevisionControlHistoryData historyData = LoadHistory(process.StandardOutput.BaseStream);

                return historyData;
            }
        }

        static public RevisionControlHistoryData LoadHistory (Stream stream)
        {
            RevisionControlHistoryData historyData = new RevisionControlHistoryData();

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

                            ReadLog(historyData, xmlReader);
                            return historyData;

                        default:
                            throw new XmlException();
                    }
                }
            }

            return null;
        }

        private static RevisionControlHistoryEntryAction ParseAction(string actionString)
        {
            switch (actionString)
            {
                case "A":
                    return RevisionControlHistoryEntryAction.Add;
                case "D":
                    return RevisionControlHistoryEntryAction.Delete;
                case "M":
                    return RevisionControlHistoryEntryAction.Modify;
                default:
                    throw new NotSupportedException();
            }
        }

        private static void ReadLog(RevisionControlHistoryData historyData, XmlReader xmlReader)
        {
            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "logentry":
                        ReadLogEntry(historyData, xmlReader);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        private static void ReadLogEntry(RevisionControlHistoryData historyData, XmlReader xmlReader)
        {
            RevisionControlHistoryEntry entry = new RevisionControlHistoryEntry();
            entry.Revision = xmlReader.GetAttribute("revision");

            xmlReader.Read();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "author":
                        entry.Author = xmlReader.ReadElementContentAsString();
                        break;
                    case "date":
                        entry.Time = xmlReader.ReadElementContentAsDateTime();
                        break;
                    case "msg":
                        entry.Message = xmlReader.ReadElementContentAsString();
                        break;
                    case "paths":
                        ReadPaths(entry, historyData, xmlReader);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            xmlReader.Read();

            historyData.AddEntry(entry);
        }

        private static void ReadPaths(
            RevisionControlHistoryEntry entry, 
            RevisionControlHistoryData historyData, 
            XmlReader xmlReader)
        {
            xmlReader.Read();
            List<RevisionControlHistoryEntryPath> pathsCollected = new List<RevisionControlHistoryEntryPath>();

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                switch (xmlReader.Name)
                {
                    case "path":
                        RevisionControlHistoryEntryPath path = new RevisionControlHistoryEntryPath();
                        path.Action = ParseAction(xmlReader.GetAttribute("action"));
                        path.Path = xmlReader.ReadElementContentAsString();
                        pathsCollected.Add(path);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            xmlReader.Read();

            entry.SetPaths(pathsCollected.ToArray());
        }

        private readonly Project project;
        private readonly string svnToolPath;// = @"C:\Program Files\CollabNet Subversion\svn.exe";
        private string svnRootPath;
    }
}