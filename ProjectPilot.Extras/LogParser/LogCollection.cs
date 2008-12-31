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

//                    while (n < lineElements.Length)
//                    {
//                        //New pattern value
//                        stringTemp.Append(lineElements[n]);
//                        n++;
//                    }

                    //Replace last element in last LogElement
                    lastEntry.Elements.RemoveAt(lastEntry.Elements.Count-1);
//                    lastEntry.Elements.Add(stringTemp.ToString());
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

//                int n = 0;
//                while (n < lineElements.Length)
//                {
//                    //New pattern value
//                    stringTemp.Append(lineElements[n]);
//                    n++;
//                }

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

//        public void ParseLogFile(Stream fileStream, Dictionary<string, Regex> pattern)
//        {
//            int n = 1;
//            string line;
//            string[] lineArray;
//            LogEntry newEntry = new LogEntry();
//
//            using (StreamReader reader = new StreamReader(fileStream))
//            {
//                while (true)
//                {
//                   line = reader.ReadLine();
//                   lineArray = line.Split(' ');
//
//                   if (pattern.ElementAt(n).Value.IsMatch(lineArray.ElementAt(n)))
//                       newEntry.LogItems.Add(pattern.ElementAt(n).Key, lineArray.ElementAt(n));
//
//                    if (line == null) //End of file.
//                        break;                       
//                }
//            }
//         }

        private CultureInfo cultureToUse = CultureInfo.InvariantCulture;

        private List<string> elementsPattern = new List<string>();
        private List<LogEntry> elementsLog = new List<LogEntry>();
        private char separator = '|';
    }
}
