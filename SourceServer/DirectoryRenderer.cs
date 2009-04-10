using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web;

namespace SourceServer
{
    public class DirectoryRenderer : IDirectoryRenderer
    {
        public string RenderDirectory(string directoryPath, DirectoryItem[] directoryItems)
        {
            this.directoryPath = directoryPath;
            this.directoryItems = directoryItems;
            this.response = new StringBuilder();

            RenderHeader();
            RenderDirectoryItems();
            RenderFooter();

            return response.ToString();
        }

        private void RenderDirectoryItems()
        {
            foreach (DirectoryItem item in directoryItems)
                RenderDirectoryItem(item);
        }

        private void RenderDirectoryItem(DirectoryItem item)
        {
            string slashedPath = SwitchFilePathToUseSlashes(item);

            response.AppendFormat("<li><a href='{0}'>", GenerateDirectoryItemUrl(item));
            if (item.IsDirectory)
                response.Append("<b>");
            response.Append(slashedPath);
            if (item.IsDirectory)
                response.Append("</b>");
            response.AppendLine("</a></li>");
        }

        private string GenerateDirectoryItemUrl(DirectoryItem directoryItem)
        {
            string properUrl = SwitchFilePathToUseSlashes(directoryItem);
            return HttpUtility.UrlEncode(properUrl);
        }

        private string SwitchFilePathToUseSlashes(DirectoryItem directoryItem)
        {
            return directoryItem.Path.Replace('\\', '/');
        }

        private void RenderFooter()
        {
            response.Append("</ul>");
            response.Append("</body></html>");
        }

        private void RenderHeader()
        {
            response.Append("<html>");
            response.Append("<head>");
            response.Append("</head>");
            response.Append("<body>");
            response.Append("<ul>");
        }

        private DirectoryItem[] directoryItems;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string directoryPath;
        private StringBuilder response;
    }
}