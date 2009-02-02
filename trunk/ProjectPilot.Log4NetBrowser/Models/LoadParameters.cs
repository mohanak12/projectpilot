using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Web;

using ProjectPilot.Extras.LogParser;

namespace ProjectPilot.Log4NetBrowser.Models
{
    public class LoadParameters
    {
        public static LogParserFilter CreateFilter(string levelSelect, 
                                          string StartTime, 
                                          string EndTime, 
                                          string ThreadId, 
                                          string numberOfItems,
                                          string searchType,
                                          string searchContent,
                                          bool logCountMode )
        {
            LogParserFilter filter = new LogParserFilter();

            bool time = true;
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            CultureInfo cultureToUse = CultureInfo.InvariantCulture;

            if (logCountMode)
                filter.LogCountMode = true;

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

            filter.FilterLevel = levelSelect;

            filter.FilterThreadId = ThreadId;

            if (!string.IsNullOrEmpty(numberOfItems))
            {
                filter.FilterNumberOfLogItems = int.Parse(numberOfItems);
            }
            else
            {
                filter.FilterNumberOfLogItems = 255;
            }

            if (!string.IsNullOrEmpty(searchContent))
            {
                if (searchType == "MatchWholeWord")
                {
                    filter.MatchWholeWordOnly = searchContent;
                }

                if (searchType == "MatchCase")
                {
                    filter.Match = searchContent;
                }
            }

            return filter;
        }
    }
}
