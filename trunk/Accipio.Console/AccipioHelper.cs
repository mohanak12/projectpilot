using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

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

        /// <summary>
        /// Validate xml document with predefined xsd schema
        /// </summary>
        /// <param name="stream">Xml file to be validated</param>
        /// <param name="xsdFileName">Path to xsd schema file</param>
        public static void ValidateXmlDocument(Stream stream, string xsdFileName)
        {
            // create and set validation settings
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            // add schema
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(null, xsdFileName);
            xmlReaderSettings.Schemas = xmlSchemaSet;
            xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // create reader
            XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings);

            // parse file
            while (xmlReader.Read())
            { }

            if (validationStatus.Length > 0)
            {
                throw new XmlException(string.Format(CultureInfo.InvariantCulture,
                    "Xml file does not match to validation schema. Details: {0}", validationStatus));
            }
        }

        /// <summary>
        /// Save validation error
        /// </summary>
        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            validationStatus.Append(string.Format(CultureInfo.InvariantCulture, "Validation Error: {0} \n", args.Message));
        }

        private static StringBuilder validationStatus = new StringBuilder(); 
    }
}
