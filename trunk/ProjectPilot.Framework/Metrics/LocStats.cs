﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.Framework.Metrics
{
    public class LocStats : ILocStats
    {
        public LocStats()
        {
        }

        public LocStatsData CountLoc(string code)
        {
            int sloc = 0;
            int cloc = 0;
            int eloc = 0;

            int isComment = 0;
            bool notEmpty = false;
            
            LocStatsData returnData = new LocStatsData(sloc, cloc, eloc);

            char tmpChar = '\0';
            char prevChar = '\0';

            for (int i = 0; i < code.Length; i++)
            {
                prevChar = tmpChar;
                tmpChar = code[i];

                // Counting comments
                if (tmpChar == '/')
                {
                    isComment++;
                    if (isComment == 2)
                        cloc++;
                }
                else
                {
                    isComment = 0;
                }

                // Counting line breaks
                if (tmpChar == '\n')
                {
                    sloc++;
                    if (notEmpty == false)
                        eloc++;
                    else
                        notEmpty = false;

                }
                else if(tmpChar != '\r')
                    notEmpty = true;
            }

            if (tmpChar != '\n') sloc++; //The last line doesn't allways end with a \n but still needs to be counted

            returnData.Cloc = cloc;
            returnData.Eloc = eloc;
            returnData.Sloc = sloc;
            
            return returnData;
        }
    }
}
