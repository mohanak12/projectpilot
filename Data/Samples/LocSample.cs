using System;
using System.Collections.Generic;
    
namespace ProjectPilot.Framework.Metrics
{
    public class LocStats : ILocStats
    {
            LocStatsData returnData = new LocStatsData(sloc, cloc, eloc);

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    else
                    {
                        sloc++;
                    }

                    /*Regex regex = new Regex("");*/
                }
            }
            return returnData;
        }

        public LocStatsData CountLocString(string code)
        {

            if (tmpChar != '\n') sloc++; //The last line doesn't allways end with a \n but still needs to be counted
        

        }

        public LocStatsData CountLocFile(string filePath)
        {
            int sloc = 0;
            int cloc = 0;
            int eloc = 0;

            /*string line = "// this is not a comment!!!";*/

            if (File.Exists(filePath))
            {
                //try
                //{
                stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                while ((tmpChar = stream.ReadByte()) != -1)
                {
                    // Counting comments
                    if (tmpChar == '\n')
                    {
                        sloc++;
                        if (notEmpty == false)
                            eloc++;
                        else
                            notEmpty = false;
                    }

                /*}/* /* /// 
                catch (Exception e)
                {////*
                    //plkjlkjlj
                }*/
            }

            //comment

            return returnData;
        }
    }
}
