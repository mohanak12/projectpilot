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

        public LogCollection(char separator, string pattern, string timePattern)
        {
            this.separator = separator;
            this.timePattern = timePattern;

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

        public CultureInfo CultureToUse
        {
            get { return cultureToUse; }
            set { cultureToUse = value; }
        }

        public IList<LogEntry> ElementsLog
        {
            get { return elementsLog; }
        }

        public IList<string> ElementsPattern
        {
            get { return elementsPattern; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "LogLine")]
        public void ParseLogLine(string line)
        {
            string []lineElements = line.Split(separator);

            if (string.IsNullOrEmpty(line))
                return;

            //Pattern in line
            if (lineElements.Length >= elementsPattern.Count)
            {
                LogEntry newEntry = new LogEntry();
                int n;

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
                        (ParsedElementBase)Assembly.GetExecutingAssembly().CreateInstance(className);

                    if (elementsPattern[n] == "Time")
                        ((TimestampElement)element).TimePattern = timePattern;

                    element.Parse(lineElements[n]);
                    newEntry.Elements.Add(element);   
                }

                elementsLog.Add(newEntry);

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
                LogEntry lastEntry = elementsLog.ElementAt(elementsLog.Count - 1);

                //Last pattern value in last LogElement
                StringBuilder stringTemp = new StringBuilder();

                stringTemp.Append(
                    ((ParsedElementBase)lastEntry.Elements.ElementAt(lastEntry.Elements.Count - 1)).Element);
                stringTemp.Append(line);
                    ((ParsedElementBase)lastEntry.Elements.ElementAt(lastEntry.Elements.Count - 1)).Parse(stringTemp.ToString());
            }
        }

        public void ParseLogFile(Stream fileStream)
        {
            string line;
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if (true == string.IsNullOrEmpty(line))
                        continue;
                    ParseLogLine(line);
                }
            }
        }

        private CultureInfo cultureToUse = CultureInfo.InvariantCulture;
        private Dictionary<string, string> conversionMap = new Dictionary<string, string>();
        private List<string> elementsPattern = new List<string>();
        private List<LogEntry> elementsLog = new List<LogEntry>();
        private char separator = '|';
        private string timePattern = "yyyy-MM-dd HH:mm:ss,fff";
    }
}
