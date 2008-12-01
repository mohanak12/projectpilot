using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
    
namespace ProjectPilot.Framework.Metrics
{
    public class LocStats : ILocStats
    {
        public LocStats()
        {
        }

        public LocStatsData CountLocString(Stream stream)
        {
            int sloc = 0;
            int cloc = 0;
            int eloc = 0;

            bool inCommentMode = false;

            LocStatsData returnData = new LocStatsData(sloc, cloc, eloc);

            using (StreamReader reader = new StreamReader(stream))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;

                    Regex regex = new Regex(@"\S");
                    
                    if (!regex.IsMatch(line))
                        eloc++;
                    
                    sloc++;
                    
                    int commentIndex = line.IndexOf(@"//", StringComparison.Ordinal);
                    int oldIndexStart = line.IndexOf(@"/*", StringComparison.Ordinal);
                    int oldIndexEnd = line.IndexOf(@"*/", StringComparison.Ordinal);
                    int quoteIndex1 = line.IndexOf("\"", StringComparison.Ordinal);
                    int quoteIndex2 = line.LastIndexOf("\"", StringComparison.Ordinal);

                    if (oldIndexStart > -1 && 
                        inCommentMode != true &&
                        !(quoteIndex1 < oldIndexStart && 
                          oldIndexStart < quoteIndex2))
                    {
                        if (commentIndex == -1 ||
                            commentIndex > oldIndexStart)
                        {
                            if (oldIndexEnd > oldIndexStart)
                                cloc++;
                            else
                                inCommentMode = true;
                        }
                    }
                    
                    if (commentIndex > -1 &&
                        (oldIndexStart == -1 || oldIndexStart > commentIndex) &&
                        inCommentMode == false &&
                        !(quoteIndex1 < commentIndex &&
                          commentIndex < quoteIndex2))
                        cloc++;

                    if (oldIndexEnd > -1 &&
                        inCommentMode == true)
                    {
                        cloc++;
                        inCommentMode = false;
                    }
                }
            }

            returnData.Cloc = cloc;
            returnData.Eloc = eloc;
            returnData.Sloc = sloc;

            return returnData;
        }

        public LocStatsData CountLocString(string code)
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
                else if (tmpChar != '\r')
                    notEmpty = true;
            }

            if (tmpChar != '\n') sloc++; //The last line doesn't allways end with a \n but still needs to be counted

            returnData.Cloc = cloc;
            returnData.Eloc = eloc;
            returnData.Sloc = sloc;

            return returnData;
        }

        public LocStatsData CountLocFile(string filePath)
        {
            int sloc = 0;
            int cloc = 0;
            int eloc = 0;

            int isComment = 0;
            bool notEmpty = false;
            bool inCommentMode = false;

            LocStatsData returnData = new LocStatsData(sloc, cloc, eloc);
            FileStream stream;

            int tmpChar = '\0';
            int prevChar = '\0';
            
            if (File.Exists(filePath))
            {
                //try
                //{
                stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                while ((tmpChar = stream.ReadByte()) != -1)
                {
                    // Counting comments
                    if (tmpChar == '/' && inCommentMode == false)
                    {
                        isComment++;
                        if (isComment == 2)
                            cloc++;
                    }
                    else if (tmpChar == '*' && 
                             prevChar == '/' &&
                             inCommentMode == false)
                    {
                        inCommentMode = true;
                        cloc++;
                    }
                    else if (tmpChar == '/' &&
                             prevChar == '*' &&
                             inCommentMode == true)
                    {
                        inCommentMode = false;
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
                    else if (tmpChar != '\r' &&
                             tmpChar != ' ' &&
                             tmpChar != '\t')
                        notEmpty = true;

                    prevChar = tmpChar;
                }

                /*}/* /* /// 
                catch (Exception e)
                {////*
                    //TODO: Ask waht to do in this case
                }*/
            }

            //comment
            returnData.Cloc = cloc;
            returnData.Eloc = eloc;
            returnData.Sloc = sloc;

            return returnData;
        }
    }
}
