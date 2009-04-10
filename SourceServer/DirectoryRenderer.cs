using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Web;

namespace SourceServer
{
    public class DirectoryRenderer : RendererBase, IDirectoryRenderer
    {
        public string RenderDirectory(string basePath, string directoryPath, DirectoryItem[] directoryItems)
        {
            this.basePath = basePath;
            this.directoryPath = directoryPath;
            this.directoryItems = directoryItems;
            Response = new StringBuilder();

            RenderHeader();
            RenderDirectoryItems();
            RenderFooter();

            return Response.ToString();
        }

        private string GenerateDirectoryItemUrl(DirectoryItem directoryItem)
        {
            string path = directoryItem.Path;
            string parentName = Path.GetFileName(Path.GetDirectoryName(path));
            string localName = Path.GetFileName(path);
            path = Path.Combine(parentName, localName);

            string properUrl = SwitchFilePathToUseSlashes(path);
            return properUrl;
            //return HttpUtility.UrlEncode(properUrl);
        }

        private void RenderDirectoryItems()
        {
            foreach (DirectoryItem item in directoryItems)
                RenderDirectoryItem(item);
        }

        private void RenderDirectoryItem(DirectoryItem item)
        {
            Response.AppendFormat(
                "<li><a href='{0}' class='{1}'>", 
                GenerateDirectoryItemUrl(item),
                item.IsDirectory ? "diritem" : "fileitem");
            Response.Append(Path.GetFileName(item.Path));
            Response.AppendLine("</a></li>");
        }

        private void RenderFooter()
        {
            Response.Append("</ul>");
            base.RenderFooter(basePath, directoryPath);
        }

        private void RenderHeader()
        {
            RenderHeader(basePath, directoryPath, directoryPath);
            Response.Append("<ul class='dirlist'>");
        }

        private static string SwitchFilePathToUseSlashes(string path)
        {
            return path.Replace('\\', '/');
        }

        private string basePath;
        private DirectoryItem[] directoryItems;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string directoryPath;
    }
}