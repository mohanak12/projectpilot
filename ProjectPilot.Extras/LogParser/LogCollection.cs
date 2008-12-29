using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectPilot.Extras.LogParser
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class LogCollection
    {
        public CultureInfo CultureToUse
        {
            get { return cultureToUse; }
            set { cultureToUse = value; }
        }

        public void ParseLogFile(Stream fileStream, Dictionary<string, Regex> pattern)
        {
            int n = 1;
            string line;
            string[] lineArray;
            LogEntry newEntry = new LogEntry();

            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (true)
                {
                   line = reader.ReadLine();
                   lineArray = line.Split(' ');

                   if (pattern.ElementAt(n).Value.IsMatch(lineArray.ElementAt(n)))
                       newEntry.LogItems.Add(pattern.ElementAt(n).Key, lineArray.ElementAt(n));

                    if (line == null) //End of file.
                        break;                       
                }
            }
         }

        private CultureInfo cultureToUse = CultureInfo.InvariantCulture;
    }
}
