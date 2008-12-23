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
        public void ParseLogFile(Stream fileStream)
        {
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) //End of file.
                        break;

                    //LogEntry newEntry = new LogEntry();

                    //string [] entryArray = line.Split(' ');

                    //newEntry.Date = line.Substring(0, line.IndexOf(@" ");
                }
            }
        }
    }
}
