using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using ProjectPilot.Framework.RevisionControlHistory;

namespace ProjectPilot.Framework.Subversion
{
    public class SubversionHistoryFacility : IRevisionControlHistoryFacility
    {
        public SubversionHistoryFacility(
            string facilityId,
            string svnToolPath, 
            string svnRootPath,
            ISessionStorage sessionStorage)
        {
            this.facilityId = facilityId;
            this.svnToolPath = svnToolPath;
            this.svnRootPath = svnRootPath;
            this.sessionStorage = sessionStorage;
        }

        public RevisionControlHistoryData FetchHistory()
        {
            // load the previously fetched history from the persistent storage
            using (ISessionState sessionState = sessionStorage.LoadSession(
                String.Format(
                    CultureInfo.InvariantCulture,
                    "SubversionHistoryFacility_{0}", 
                    facilityId)))
            {
                RevisionControlHistoryData lastFetchedHistory
                    = sessionState.GetValue<RevisionControlHistoryData>(SessionKeyLastFetchedHistory);

                // find the last revision that was fetched
                string lastRevisionFetched = null;

                if (lastFetchedHistory != null)
                    lastRevisionFetched = lastFetchedHistory.LastRevision;

                // svn log d:\svn\mobilkom.nl-bhwr\trunk\src -v --xml --non-interactive >D:\BuildArea\builds\mobilkom.nl-bhwr\svn-log.xml
                // -r 1729:HEAD

                using (Process process = new Process())
                {
                    StringBuilder argumentsBuilder = new StringBuilder();
                    argumentsBuilder.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"log ""{0}"" --xml --non-interactive",
                        svnRootPath);

                    // add revisions range, if we have history data fetched previously
                    if (lastRevisionFetched != null)
                    {
                        int lastRevisionFetchedInt = Int32.Parse(lastRevisionFetched, CultureInfo.InvariantCulture);
                        argumentsBuilder.AppendFormat(
                            CultureInfo.InvariantCulture,
                            " -r {0}:HEAD", 
                            lastRevisionFetchedInt + 1);
                    }

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.FileName = svnToolPath;
                    process.StartInfo.Arguments = argumentsBuilder.ToString();
                    process.Start();

                    process.WaitForExit();

                    // merge the two histories
                    RevisionControlHistoryData historyData = LoadHistory(process.StandardOutput.BaseStream);
                    if (lastFetchedHistory != null)
                        historyData.Merge(lastFetchedHistory);
                    
                    // save the new history to the session state
                    sessionState.SetValue(SessionKeyLastFetchedHistory, historyData);

                    return historyData;
                }
            }
        }

        public static RevisionControlHistoryData LoadHistory(Stream stream)
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
                case "R":
                    return RevisionControlHistoryEntryAction.Replace;
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
                        ReadPaths(entry, xmlReader);
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

        private readonly ISessionStorage sessionStorage;
        private readonly string facilityId;
        private const string SessionKeyLastFetchedHistory = "LastFetchedHistory";
        private readonly string svnToolPath;// = @"C:\Program Files\CollabNet Subversion\svn.exe";
        private string svnRootPath;
    }
}