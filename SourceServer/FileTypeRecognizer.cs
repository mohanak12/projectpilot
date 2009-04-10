using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace SourceServer
{
    public class FileTypeRecognizer : IFileTypeRecognizer
    {
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public string RecognizeFileType(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower(CultureInfo.InvariantCulture);
            switch (extension)
            {
                case ".cpp":
                case ".h":
                    return "cpp";
                case ".cs":
                    return "c#";
                case ".css":
                    return "css";
                case ".java":
                    return "java";
                case ".js":
                    return "js";
                case ".php":
                    return "php";
                case ".sql":
                    return "sql";
                case ".vb":
                    return "vb";
                case ".htm":
                case ".html":
                case ".xml":
                case ".xsd":
                case ".xsl":
                case ".xslt":
                    return "xml";
                default:
                    return null;
            }
        }
    }
}