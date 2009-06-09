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
                    return "csharp";
                case ".css":
                    return "css";
                case ".java":
                    return "java";
                case ".js":
                    return "jscript";
                case ".php":
                    return "php";
                case ".sql":
                    return "sql";
                case ".vb":
                case ".asp":
                case ".bas":
                case ".cls":
                    return "vb";
                case ".htm":
                case ".html":
                case ".xml":
                case ".xsd":
                case ".xsl":
                case ".xslt":
                case ".wsdl":
                case ".build":
                    return "xml";
                case ".dll":
                case ".exe":
                    return null;
                default:
                    return "plain";
            }
        }
    }
}