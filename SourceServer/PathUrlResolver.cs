using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using log4net;

namespace SourceServer
{
    public class PathUrlResolver
    {
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public string ResolveUrl(Uri url)
        {
            string path = url.LocalPath;

            int start = 0;
            for (int i = 0; i < 2; i++)
            {
                start = path.IndexOf("/", start, StringComparison.OrdinalIgnoreCase);
                if (start == -1)
                    return null;

                start++;
            }

            string realLocalPath = path.Substring(start);
            
            if (realLocalPath.ToLower(CultureInfo.InvariantCulture).Contains("sourceserver.css"))
                realLocalPath = null;

            if (log.IsDebugEnabled)
                log.DebugFormat("'{0}' resolved to '{1}'", url, realLocalPath);

            return realLocalPath;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(PathUrlResolver));
    }
}