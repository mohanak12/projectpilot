using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
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

            conversionMap.Add("Time", new TimestampElement());
            conversionMap.Add("ThreadId", new ThreadIdElement());
            conversionMap.Add("Level", new LevelElement());
            conversionMap.Add("Message", new MessageElement());
            conversionMap.Add("Ndc", new NdcElement());
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

                //Add's pattern values
                for (n = 0; n < elementsPattern.Count; n++)
                {
                    //object element = conversionMap.TryGetValue(elementsPattern[n, ); !!
                    //newEntry.Elements.Add(element);
                    newEntry.Elements.Add(lineElements[n]);  
                }

                elementsLog.Add(newEntry);

                //Add's last pattern
                if (n < lineElements.Length)
                {
                    int count = 0;
                    for (n=n-2; n >= 0; n--)
                        count = count + lineElements[n].Length;
                    
                    //count parsed separator symbols
                    count = count + elementsPattern.Count -1;

                    //Last LogElement
                    LogEntry lastEntry = elementsLog.ElementAt(elementsLog.Count - 1);
                    
                    //Last pattern value in last LogElement
                    StringBuilder stringTemp = new StringBuilder();
                    stringTemp.Append(lastEntry.Elements.ElementAt(lastEntry.Elements.Count - 1));

                    //Replace last element in last LogElement
                    lastEntry.Elements.RemoveAt(lastEntry.Elements.Count-1);
                    lastEntry.Elements.Add(line.Substring(count));

                    //Replace last LogElement
                    elementsLog.RemoveAt(elementsLog.Count - 1);
                    elementsLog.Add(lastEntry);
                }
            }
            else
            {   //No pattern in line  - value it's added to previus LogElement (to last pattern element of previus LogElement)
                //Last LogElement
                LogEntry lastEntry = elementsLog.ElementAt(elementsLog.Count - 1);

                //Last pattern value in last LogElement
                StringBuilder stringTemp = new StringBuilder();
                stringTemp.Append(lastEntry.Elements.ElementAt(lastEntry.Elements.Count - 1));
                stringTemp.Append(line);

                //Replace last element in last LogElement
                lastEntry.Elements.RemoveAt(lastEntry.Elements.Count - 1);
                lastEntry.Elements.Add(stringTemp.ToString());
                
                //Replace last LogElement
                elementsLog.RemoveAt(elementsLog.Count - 1);
                elementsLog.Add(lastEntry);
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
        private Dictionary<string, object> conversionMap = new Dictionary<string, object>();
        private List<string> elementsPattern = new List<string>();
        private List<LogEntry> elementsLog = new List<LogEntry>();
        private char separator = '|';
    }
}
