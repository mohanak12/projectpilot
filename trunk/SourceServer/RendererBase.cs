using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace SourceServer
{
    public abstract class RendererBase
    {
        protected StringBuilder Response
        {
            get { return response; }
            set { response = value; }
        }

        protected string ConvertToWebSafePath (string unsafePath)
        {
            return HttpUtility.UrlPathEncode(SwitchFilePathToUseSlashes(unsafePath));
        }

        protected virtual void RenderFooter(string basePath, string path)
        {
            response.Append("</div>");
            response.Append("</body></html>");
            response.Append("<div class='footer'>");
            RenderBreadcrumbs(basePath, path);
            response.Append("</div>");
        }

        protected virtual void RenderHeader(string basePath, string pageTitle, string path)
        {
            response.Append("<html>");
            response.Append("<head>");
            response.AppendFormat("<title>{0}</title>", pageTitle);
            response.AppendFormat("<link type='text/css' rel='stylesheet' href='{0}/SourceServer.css'/>", basePath);
            response.Append("</head>");
            response.Append("<body>");
            response.Append("<div class='header'>");
            RenderBreadcrumbs(basePath, path);
            response.Append("</div>");
            response.Append("<div class='file'>");
        }

        private void RenderBreadcrumbs(string basePath, string path)
        {
            List<string> crumbs = new List<string>();
            List<string> crumbsLinks = new List<string>();

            string leftOverPath = path;
            while (leftOverPath != null)
            {
                string rightPart = Path.GetFileName(leftOverPath);

                if (String.IsNullOrEmpty(rightPart))
                    break;

                crumbs.Insert(0, rightPart);
                crumbsLinks.Insert(0, ConvertToWebSafePath(leftOverPath));

                try
                {
                    leftOverPath = Path.GetDirectoryName(leftOverPath);
                }
                catch (ArgumentException)
                {
                    break;
                }
            }

            response.Append("<h2>");
            response.AppendFormat("<a href='{0}'>root</a>", basePath);
            for (int i = 0; i < crumbs.Count; i++)
            {
                response.AppendFormat(
                    " / <a href='{2}/{0}'>{1}</a>",
                    crumbsLinks[i],
                    crumbs[i],
                    basePath);
            }

            response.Append("</h2>");
        }

        private static string SwitchFilePathToUseSlashes(string path)
        {
            return path.Replace('\\', '/');
        }

        private StringBuilder response;
    }
}