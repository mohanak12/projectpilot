using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Accipio.Console
{
    /// <summary>
    /// Generate business actions XML schema file.
    /// </summary>
    public class BusinessActionGenerator : IGenerator
    {
        public void Parse(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Missing file name.");
            }

            // read xml file
            using (Stream stream = AccipioHelper.GetXmlFileContent(args[0]))
            {
                ValidateXmlDocument(stream);
            }




            //validating XML with schema file (automatic)

            //parsing XML file and retrieving TestActions, parameters etc
            //XmlTestSpecsParser parser = new XmlTestSpecsParser(content);
            //TestSpecs testSpecs = parser.Parse();

            //using (ICodeWriter writer = new FileCodeWriter(OutputFile))
            //{
            //    ITestCodeGenerator cSharpCode = new CSharpTestCodeGenerator(writer);
            //    cSharpCode.Generate(testSpecs);
            //}
            //generating XSD file which contains these actions

            //return true;
        }

        /// <summary>
        /// Validate xml document with predefined xsd schema
        /// </summary>
        /// <param name="stream">Xml file to be validated</param>
        private void ValidateXmlDocument(Stream stream)
        {
            // create and set validation settings
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            // add schema
            XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
            xmlSchemaSet.Add(null, @"..\..\..\Data\Samples\AccipioActions.xsd");
            xmlReaderSettings.Schemas = xmlSchemaSet;
            xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler(this.ValidationCallBack);

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
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            validationStatus.Append(string.Format(CultureInfo.InvariantCulture, "Validation Error: {0} \n", args.Message));
        } 

        public void Process()
        {
            throw new NotImplementedException();
        }

        public string OutputFile { get; set; }

        private StringBuilder validationStatus = new StringBuilder(); 
    }
}