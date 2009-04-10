using System;

namespace SourceServer
{
    public class PathUrlResolver
    {
        public string ResolveUrl (Uri url)
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

            return path.Substring(start);
        }
    }
}