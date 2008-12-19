using System;
using System.IO;

namespace ProjectPilot.Framework.Metrics
{
    public class AspxLocStats : ILocStats
    {
        public LocStatsData CountLocStream(Stream stream)
        {
            int sloc = 0;
            int cloc = 0;
            int eloc = 0;

            bool inCommentMode = false;

            using (StreamReader reader = new StreamReader(stream))
            {
                //Looping through the file stream.
                while (true)
                {
                    string line = reader.ReadLine();
                    
                    if (line == null) //End of file.
                        break;

                    if (line.Trim().Length == 0) //Empty line
                        eloc++;

                    sloc++; //Incrementig the sloc at each iteration.

                    //if we are in a multi line comment
                    if (inCommentMode == true)
                        cloc++;
                }
            }
            
            LocStatsData returnData = new LocStatsData(sloc, cloc, eloc);
            return returnData;
        }
    }
}
