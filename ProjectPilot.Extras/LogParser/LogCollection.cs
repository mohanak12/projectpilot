using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public LogCollection(string patternString)
        {
            this.patternArray = patternString.Split(' ');
        }

        public void ParseLogFile(Stream fileStream)
        {
            string line = "date time threadId level other";
            string[] parseArray = line.Split(' ');

            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (true)
                {
                   line = reader.ReadLine();
                    if (line == null) //End of file.
                        break;

                    LogEntry newItem = new LogEntry();

                    string[] lineArray = line.Split(' ');

                    bool dataFlag = false;

                    for (int n=0; n < parseArray.Length; n++)
                    {
                        switch (parseArray.ElementAt(n))
                        {
                            case "date":
                                if (regexDate.IsMatch((lineArray.ElementAt(n))))
                                {
                                    dataFlag = true;
                                }
                                 
                                if (regexDate2.IsMatch((lineArray.ElementAt(n))))
                                {
                                    dataFlag = true;
                                }

                                if(dataFlag == true)
                                {
                                    newItem.LogItems.Add(parseArray.ElementAt(n), lineArray.ElementAt(n));
                                    dataFlag = false;
                                }

                                continue;
                            case "time":

                                continue;
                            case "threadid":

                                continue;
                            case "level":

                                continue;
                            default:
                                break;
                                //
                        }
                    }

//                    logGroup.Add(newEntry);
                }
            }
         }

        private static readonly Regex regexDate = new Regex(@"^\d{4}-\d{2}-\d{2}$");
        private static readonly Regex regexDate2 = new Regex(@"^\d{2}-\d{2}-\d{4}$");

//      private List<LogEntry> logGroup = new List<LogEntry>();

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string [] patternArray;
    }
 }
