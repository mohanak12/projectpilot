using System;
using System.IO;
using System.Text;
using System.Web;

namespace SourceServer
{
    // http://shjs.sourceforge.net/
    // http://code.google.com/p/syntaxhighlighter/

    public class SourceCodeRenderer : RendererBase, ISourceCodeRenderer
    {
        public string Render(string basePath, string sourceCode, string fileName, string fileType)
        {
            this.basePath = basePath;
            this.fileName = fileName;
            this.sourceCode = sourceCode;
            this.fileType = fileType;

            Response = new StringBuilder();
            RenderHeader();

            if (sourceCode != null)
            {
                RenderLineNumbers();
                RenderSourceCodeLines();
            }
            else
                RenderDoesNotExistMessage();

            RenderFooter();
            return Response.ToString();
        }

        protected override void RenderHtmlHead()
        {
            base.RenderHtmlHead();
            Response.AppendFormat(
                @"	<script type='text/javascript' src='{0}/SS_stuff/scripts/shCore.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushCpp.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushCSharp.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushCss.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushJava.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushJScript.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushPhp.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushPlain.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushSql.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushVb.js'></script>
	<script type='text/javascript' src='{0}/SS_stuff/scripts/shBrushXml.js'></script>
	<link type='text/css' rel='stylesheet' href='{0}/SS_stuff/styles/shCore.css'/>
	<link type='text/css' rel='stylesheet' href='{0}/SS_stuff/styles/shThemeDefault.css'/>
	<script type='text/javascript'>
		SyntaxHighlighter.config.clipboardSwf = '{0}/SS_stuff/scripts/clipboard.swf';
		SyntaxHighlighter.all();
	</script>", 
              basePath);
        }

        private void ReadLine()
        {
            lineCounter++;
            currentLine = reader.ReadLine();
        }

        private void RenderDoesNotExistMessage()
        {
            Response.AppendLine("Looking for it? Well, you won't find it because it doesn't exist... tough luck");
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
                Response.Append(HttpUtility.HtmlEncode(currentLine));

            Response.AppendLine();
        }

        private void RenderLineNumbers()
        {
            Response.Append("<div class='divln'><pre>");

            using (reader = new StringReader(sourceCode))
            {
                while (reader.Peek() != -1)
                {
                    ReadLine();
                    RenderLineNumber();
                }
            }

            Response.Append("</pre></div>");
        }

        private void RenderLineNumber()
        {
            string trimmedLine = currentLine.Trim();
            if (trimmedLine.Length > 0)
            {
                Response.AppendFormat("<a href='#{0}' class='ln'>{0,5}</a>", lineCounter);
                Response.AppendFormat("<a name='{0}'>  </a>", lineCounter);
            }

            Response.AppendLine();
        }

        private void RenderSourceCodeLines()
        {
            Response.AppendFormat(
                "<div class='divcode'><pre class='brush:{0};gutter:false'>",
                fileType);

            using (reader = new StringReader(sourceCode))
            {
                while (reader.Peek() != -1)
                {
                    ReadLine();
                    RenderLine();
                }
            }

            Response.Append("</pre></div>");
        }

        private string basePath;
        private string currentLine;
        private string fileName;
        private string fileType;
        private int lineCounter;
        private StringReader reader;
        private string sourceCode;
    }
}