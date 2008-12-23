using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectPilot.Extras.LogParser
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class LogCollection
    {
        public LogCollection(string patternString)
        {
            this.patternArray = patternString.Split(' ');
        }

        public void ParseLogFile(Stream fileStream)
        {
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) //End of file.
                        break;

                    LogEntry newEntry = new LogEntry();
                    newEntry.ParseEntry(patternArray, line);

                    logGroup.Add(newEntry);
                }
            }
         }

        private List<LogEntry> logGroup = new List<LogEntry>();
        private string [] patternArray;
    }
 }
