using System;
using System.IO;
using System.Text;
using System.Web;

namespace SourceServer
{
    // http://shjs.sourceforge.net/
    // http://code.google.com/p/syntaxhighlighter/

    public class PlainSourceCodeRenderer : ISourceCodeRenderer
    {
        public string Render(string sourceCode, string fileName, string fileType)
        {
            using (reader = new StringReader(sourceCode))
            {
                response = new StringBuilder();

                RenderHeader();

                while (reader.Peek() != -1)
                {
                    ReadLine();
                    RenderLine();
                }

                RenderFooter();

                return response.ToString();
            }
        }

        private void ReadLine()
        {
            lineCounter++;
            currentLine = reader.ReadLine();
        }

        private void RenderFooter()
        {
            response.Append("</pre>");
            response.Append("</body></html>");
        }

        private void RenderHeader()
        {
            response.Append("<html>");
            response.Append("<head>");
            response.Append(
                @"<style type='text/css'>
A:link {text-decoration: none}
A:visited {text-decoration: none}
A:active {text-decoration: none}
A:hover {text-decoration: underline; color: red;}
</style>");
            response.Append("</head>");
            response.Append("<body>");
            response.Append("<pre>");
        }

        private void RenderLine()
        {
            string trimmedLine = currentLine.Trim();
            if (trimmedLine.Length > 0)
            {
                response.AppendFormat("<a href='#{0}'>{0,5}</a>", lineCounter);
                response.AppendFormat("<a name='{0}'>  </a>", lineCounter);
                response.AppendLine(HttpUtility.HtmlEncode(currentLine));
            }
            else
                response.AppendLine();
        }

        private int lineCounter;
        private string currentLine;
        private StringReader reader;
        private StringBuilder response;
    }
}