using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ProjectPilot.Extras.LogParser;

namespace ProjectPilot.Log4NetBrowser.Models
{
    public class Filter
    {
         public static LogParserFilter CreateFilter(
                                          string StartTime, 
                                          string EndTime, 
                                          string ThreadId,
                                          string level,
                                          string searchContent,
                                          bool searchWholeWord,
                                          int? searchNumberOfItems,
                                          int? startSearchIndex,
                                          int? endSearchIndex,
                                          int? startSearchByte,
                                          int? endSearchByte
             )
         {
             LogParserFilter filter = new LogParserFilter();

             //Time
             bool time = true;
             DateTime startTime = new DateTime();
             DateTime endTime = new DateTime();
             CultureInfo cultureToUse = CultureInfo.InvariantCulture;

             if (string.IsNullOrEmpty(StartTime) && string.IsNullOrEmpty(EndTime))
             {
                 StartTime = "";
                 EndTime = "";
             }

             try
             {
                 startTime = DateTime.ParseExact(StartTime, "dd.MM.yyyy HH:mm:ss,fff", cultureToUse);
             }
             catch (FormatException)
             {
                 time = false;
             }

             try
             {
                 endTime = DateTime.ParseExact(EndTime, "dd.MM.yyyy HH:mm:ss,fff", cultureToUse);
             }
             catch (FormatException)
             {
                 time = false;
             }

             if (time)
             {
                 filter.FilterTimestampStart = startTime;
                 filter.FilterTimestampEnd = endTime;
             }


             //ThreadId
             filter.FilterThreadId = ThreadId;

             //Level
             filter.FilterLevel = level;
             
             //Search
             if (!string.IsNullOrEmpty(searchContent))
             {
                 if (searchWholeWord)
                 {
                     filter.MatchWholeWordOnly = searchContent;
                 }
                 else
                 {
                     filter.Match = searchContent;
                 }
             }

             //NumberOfItems
             filter.FilterNumberOfLogItems = searchNumberOfItems;

             //IndexSearch
             filter.StartLogIndex = startSearchIndex;
             filter.EndLogIndex = endSearchIndex;

             //File
             filter.ReadIndexStart = startSearchByte;
             filter.ReadIndexEnd = endSearchByte;

             return filter;
         }
    }
}
