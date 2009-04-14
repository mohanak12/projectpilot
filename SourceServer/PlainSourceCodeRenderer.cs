using System;
using System.IO;
using System.Text;
using System.Web;

namespace SourceServer
{
    // http://shjs.sourceforge.net/
    // http://code.google.com/p/syntaxhighlighter/

    public class PlainSourceCodeRenderer : RendererBase, ISourceCodeRenderer
    {
        public string Render(string basePath, string sourceCode, string fileName, string fileType)
        {
            this.basePath = basePath;
            this.fileName = fileName;

            Response = new StringBuilder();
            RenderHeader();

            if (sourceCode != null)
            {
                Response.Append("<pre>");

                using (reader = new StringReader(sourceCode))
                {
                    while (reader.Peek() != -1)
                    {
                        ReadLine();
                        RenderLine();
                    }
                }

                Response.Append("</pre>");
            }
            else
                RenderDoesNotExistMessage();

            RenderFooter();
            return Response.ToString();
        }

        private void RenderDoesNotExistMessage()
        {
            Response.AppendLine("Looking for it? Well, you won't find it because it doesn't exist... tough luck");
        }

        private void ReadLine()
        {
            lineCounter++;
            currentLine = reader.ReadLine();
        }

        private void RenderFooter()
        {
            base.RenderFooter(basePath, fileName);
        }

        private void RenderHeader()
        {
            RenderHeader(basePath, fileName, fileName);
        }

        private void RenderLine()
        {
            string trimmedLine = currentLine.Trim();
            if (trimmedLine.Length > 0)
            {
                Response.AppendFormat("<div class='dln'><a href='#{0}' class='ln'>{0,5}</a>", lineCounter);
                Response.AppendFormat("<a name='{0}'>  </a></div>", lineCounter);
                Response.AppendFormat("<div class='dc{0}'>", lineCounter % 2);
                Response.Append(HttpUtility.HtmlEncode(currentLine));
                Response.AppendLine("</div><div style='clear:left;margins:0px'/>");
            }
            else
            {
                Response.Append("<div class='dln'>       </div>");
                Response.AppendFormat("<div class='dc{0}'>&nbsp;", lineCounter % 2);
                Response.AppendLine("</div>");
            }   
        }

        private string basePath;
        private string currentLine;
        private string fileName;
        private int lineCounter;
        private StringReader reader;
    }
}