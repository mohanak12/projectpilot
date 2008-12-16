using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Accipio.Console
{
    /// <summary>
    /// XmlValidationHelper class contains helper methods
    /// </summary>
    public class XmlValidationHelper
    {
        /// <summary>
        /// Validate xml document with predefined xsd schema
        /// </summary>
        /// <param name="xmlFileName">File of the XML file to be validated.</param>
        /// <param name="schemaFileName">Path to xsd schema file.</param>
        public void ValidateXmlDocument(string xmlFileName, string schemaFileName)
        {
            validationExceptions.Clear();

            // create and set validation settings
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            // add schema
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(null, schemaFileName);
            xmlReaderSettings.Schemas = xmlSchemaSet;
            xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            using (Stream stream = File.OpenRead(xmlFileName))
            {
                // create reader
                using (XmlReader xmlReader = XmlReader.Create(stream, xmlReaderSettings))
                {
                    // parse file
                    while (xmlReader.Read())
                    {
                    }
                }

                if (validationExceptions.Count > 0)
                {
                    throw validationExceptions[0];
                }
            }
        }

        /// <summary>
        /// Callback method for XML validation.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">Event arguments.</param>
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            validationExceptions.Add(args.Exception);
        }

        private readonly List<XmlSchemaException> validationExceptions = new List<XmlSchemaException>();
    }
}
