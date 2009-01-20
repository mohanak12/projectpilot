using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProjectPilot.Extras.LogParser
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class LogCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogCollection"/> class.
        /// </summary>
        /// <param name="separator">The separator between log items.</param>
        /// <param name="pattern">The pattern used for parsing log file.</param>
        public LogCollection(char separator, string pattern)
        {
            this.separator = separator;
            string[] elementsTemp = pattern.Split(separator);

            foreach (string elementInPattern in elementsTemp)
            {
                elementsPattern.Add(elementInPattern);
            }

            conversionMap.Add("Time", typeof(TimestampElement).FullName);
            conversionMap.Add("ThreadId", typeof(ThreadIdElement).FullName);
            conversionMap.Add("Level", typeof(LevelElement).FullName);
            conversionMap.Add("Message", typeof(MessageElement).FullName);
            conversionMap.Add("Ndc", typeof(NdcElement).FullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogCollection"/> class.
        /// </summary>
        /// <param name="separator">The separator between log items.</param>
        /// <param name="pattern">The pattern used for parsing log file.</param>
        /// <param name="timePattern">The time pattern used for parsing time in log file.</param>
        /// <param name="cultureToUse">The culture used for parsing time in log file.</param>
        public LogCollection(char separator, string pattern, string timePattern, CultureInfo cultureToUse)
        {
            this.separator = separator;
            this.timePattern = timePattern;
            this.cultureToUse = cultureToUse;

            string[] elementsTemp = pattern.Split(separator);

            foreach (string elementInPattern in elementsTemp)
            {
                elementsPattern.Add(elementInPattern);
            }

            conversionMap.Add("Time", typeof(TimestampElement).FullName);
            conversionMap.Add("ThreadId", typeof(ThreadIdElement).FullName);
            conversionMap.Add("Level", typeof(LevelElement).FullName);
            conversionMap.Add("Message", typeof(MessageElement).FullName);
            conversionMap.Add("Ndc", typeof(NdcElement).FullName);
        }

        public IList<LogEntry> ElementsLog
        {
            get { return elementsLog; }
        }

        public IList<string> ElementsPattern
        {
            get { return elementsPattern; }
        }

        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly")]
        public LogParserFilter ParseFilter
        {
            set { parseFilter = value; }
        }

        /// <summary>
        /// Parses the log line.
        /// </summary>
        /// <param name="line">The line of log file.</param>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "LogLine")]
        public void ParseLogLine(string line)
        {
            //Log Index Filter (log entries from X to Y)
            if (parseFilter.StartLogIndex != null)
                if (currentIndex < parseFilter.StartLogIndex)
                {
                    currentIndex++;
                    return;
                }

            if (parseFilter.EndLogIndex != null)
                if (currentIndex > parseFilter.EndLogIndex)
                {
                    stopFlag = true;
                    return;
                }

            string []lineElements = line.Split(separator);
            int n;

            //Pattern head in log line
            bool patternHeadInLine = true;
           
            for (n = 0; n < elementsPattern.Count; n++)
            {
                if (elementsPattern[n] == "Time")
                {
                    string a = lineElements[n];
                    
                    try
                    {
                        DateTime test = DateTime.ParseExact(lineElements[n], timePattern, cultureToUse);
                    }
                    catch (FormatException)
                    {
                        patternHeadInLine = false;
                    } 
                }
            }

            //Pattern in line
            if (lineElements.Length >= elementsPattern.Count && patternHeadInLine == true)
            {
                LogEntry newEntry = new LogEntry();

                //Add's values to pattern
                for (n = 0; n < elementsPattern.Count; n++)
                {
                    if (false == conversionMap.ContainsKey(elementsPattern[n]))
                        throw new KeyNotFoundException(
                            String.Format(
                                CultureInfo.InvariantCulture,
                                "The pattern '{0}' is not supported.",
                                elementsPattern[n]));

                    string className = conversionMap[elementsPattern[n]];

                    ParsedElementBase element =
                        (ParsedElementBase) Assembly.GetExecutingAssembly().CreateInstance(className);

                    if (elementsPattern[n] == "Time")
                    {
                        ((TimestampElement) element).TimePattern = timePattern;
                        ((TimestampElement) element).CultureToUse = cultureToUse;
                    }

                    element.Parse(lineElements[n]);
                    newEntry.Elements.Add(element);
                }

                DateTime? timestamp = null;
                string level = string.Empty;
                string threadId = string.Empty;

                if (elementsPattern.IndexOf("Time") >= 0)
                {
                    TimestampElement timestampElement =
                        (TimestampElement) newEntry.Elements.ElementAt(elementsPattern.IndexOf("Time"));
                    timestamp = (DateTime) timestampElement.Element;
                }

                if (elementsPattern.IndexOf("Level") >= 0)
                {
                    LevelElement levelElement =
                        (LevelElement) newEntry.Elements.ElementAt(elementsPattern.IndexOf("Level"));
                    level = (string) levelElement.Element;
                }

                if (elementsPattern.IndexOf("ThreadId") >= 0)
                {
                    ThreadIdElement threadIdElement =
                        (ThreadIdElement) newEntry.Elements.ElementAt(elementsPattern.IndexOf("ThreadId"));
                    threadId = (string) threadIdElement.Element;
                }

                if (!Filter(timestamp, threadId, level, ElementsLog.Count))
                {
                    addTolastPattern = false;
                    return;
                }

                addTolastPattern = true;
                elementsLog.Add(newEntry);
                currentIndex++;
                //Add's last pattern
                if (n < lineElements.Length)
                {
                    int count = 0;
                    for (n=n-2; n >= 0; n--)
                        count = count + lineElements[n].Length;
                    
                    //count parsed (+separator) symbols
                    count = count + elementsPattern.Count - 1;

                    //Last LogElement
                    LogEntry lastEntry = elementsLog.ElementAt(elementsLog.Count - 1);

                    //New log value
                    ((ParsedElementBase)lastEntry.Elements.ElementAt(lastEntry.Elements.Count - 1)).Parse(line.Substring(count));
                }
            }
            else
            {   //No pattern in line  - value it's added to previus LogElement (to last pattern element of previus LogElement)
                //Last LogElement
                if (addTolastPattern == false) return;
                LogEntry lastEntry = elementsLog.ElementAt(elementsLog.Count - 1);

                //Last pattern value in last LogElement
                StringBuilder stringTemp = new StringBuilder();

                stringTemp.Append(
                    ((ParsedElementBase)lastEntry.Elements.ElementAt(lastEntry.Elements.Count - 1)).Element);
                stringTemp.Append(line);
                    ((ParsedElementBase)lastEntry.Elements.ElementAt(lastEntry.Elements.Count - 1)).Parse(stringTemp.ToString());
            }
        }

        /// <summary>
        /// Parses the log file.
        /// </summary>
        /// <param name="fileStream">The stream of the file.</param>
        public void ParseLogFile(Stream fileStream)
        {
            string line;
            
          //  if (elementsPattern.Count <= 1) return;

            using (StreamReader reader = new StreamReader(fileStream))
            {
                if (parseFilter.StartLogIndex > parseFilter.EndLogIndex)
                    stopFlag = true;

                while (!reader.EndOfStream && stopFlag == false)
                {
                    line = reader.ReadLine();
                    if (true == string.IsNullOrEmpty(line))
                        continue;
                    ParseLogLine(line);
                }
            }
        }

        private bool Filter(DateTime? timethread, string threadId, string level, int? count)
        {
            bool filterFlag = true;

            if (parseFilter.FilterTimestampStart.HasValue && parseFilter.FilterTimestampEnd.HasValue)
            {
                if (DateTime.Compare((DateTime)timethread, (DateTime)parseFilter.FilterTimestampStart) < 0 ||
                    DateTime.Compare((DateTime)timethread, (DateTime)parseFilter.FilterTimestampEnd) > 0)
                {
                    filterFlag = false;
                }
            }

            if (!String.IsNullOrEmpty(parseFilter.FilterThreadId))
            {
                if (parseFilter.FilterThreadId != threadId)
                    filterFlag = false;
            }

            if (!String.IsNullOrEmpty(parseFilter.FilterLevel))
            {
                if (parseFilter.FilterLevel != level)
                    filterFlag = false;
            }

            if (parseFilter.FilterNumberOfLogItems != null)
            {
                if (count >= parseFilter.FilterNumberOfLogItems)
                {
                    filterFlag = false;
                    stopFlag = true;
                }
            }

            if (filterFlag)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool addTolastPattern = false;
        private CultureInfo cultureToUse = CultureInfo.InvariantCulture;
        private int currentIndex;
        private Dictionary<string, string> conversionMap = new Dictionary<string, string>();
        private List<string> elementsPattern = new List<string>();
        private List<LogEntry> elementsLog = new List<LogEntry>();
        private char separator = '|';
        private bool stopFlag;
        private string timePattern = "yyyy-MM-dd HH:mm:ss,fff";
        private LogParserFilter parseFilter = new LogParserFilter();
    }
}
