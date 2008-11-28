using System;
using System.IO;

namespace Accipio.Console
{
    /// <summary>
    /// AccipioHelper class contains helper methods
    /// </summary>
    public static class AccipioHelper
    {
        /// <summary>
        /// Gets the content from xml file.
        /// </summary>
        /// <param name="fileName">Xml file name to be readed as stream</param>
        /// <returns>Content of xml file as stream</returns>
        public static Stream GetXmlFileContent(string fileName)
        {
            string fileShema = fileName;
            FileInfo fileInfoShema = new FileInfo(fileShema);
            if (File.Exists(fileInfoShema.FullName))
            {
                if (fileInfoShema.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    return File.OpenRead(fileInfoShema.FullName);
                }

                if (fileInfoShema.Extension.Equals(".xsd", StringComparison.OrdinalIgnoreCase))
                {
                    return File.OpenRead(fileInfoShema.FullName);
                }
            }

            return null;
        }
    }
}
