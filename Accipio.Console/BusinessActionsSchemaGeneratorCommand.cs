using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Accipio.Console
{
    /// <summary>
    /// Generate business actions XML schema file.
    /// </summary>
    public class BusinessActionsSchemaGeneratorCommand : IConsoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the BusinessActionsSchemaGeneratorCommand class.
        /// </summary>
        /// <param name="nextCommandInChain">Application arguments</param>
        public BusinessActionsSchemaGeneratorCommand(IConsoleCommand nextCommandInChain)
        {
            this.nextCommandInChain = nextCommandInChain;
        }

        /// <summary>
        /// Gets output file name
        /// </summary>
        public string OutputFile
        {
            get { return outputFileName; }
        }

        /// <summary>
        /// Returns the first <see cref="IConsoleCommand"/> in the command chain
        /// which can understand the provided command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>
        /// The first <see cref="IConsoleCommand"/> which can understand the provided command-line arguments
        /// or <c>null</c> if none of the console commands can understand them.
        /// </returns>
        public IConsoleCommand ParseArguments(string[] args)
        {
            if (args == null)
                return null;

            if (args.Length < 1
                || 0 != String.Compare(args[0], "baschema", StringComparison.OrdinalIgnoreCase))
            {
                if (nextCommandInChain != null)
                    return nextCommandInChain.ParseArguments(args);
                return null;
            }

            if (args.Length < 2)
                throw new ArgumentException("Missing business actions XML file name.");

            // set xml file name
            businessActionsXmlFileName = args[1];

            FileInfo fileInfo = new FileInfo(businessActionsXmlFileName);

            // check if file exists
            if (!fileInfo.Exists)
                throw new System.IO.IOException(string.Format(CultureInfo.InvariantCulture, "File {0} does not exists.", businessActionsXmlFileName));

            // set output file name
            outputFileName = Path.ChangeExtension(fileInfo.Name, "xsd");

            return this;
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        public void ProcessCommand()
        {
            // create instance of class XmlValidationHelper
            XmlValidationHelper helper = new XmlValidationHelper();
            // validating XML with schema file
            // xml document must have at least one action element
            helper.ValidateXmlDocument(businessActionsXmlFileName, AccipioActionsXsdFileName);

            // parse XML file
            BusinessActionData businessActionData = ParseXmlToObject(businessActionsXmlFileName);

            // generating XSD schema file with business actions validation parameters
            XmlDocument xmlSchemaDocument = GenerateXsdSchema(businessActionData);

            // write xsd schema to file
            using (Stream xsdSchemaDocument = File.OpenWrite(outputFileName))
            {
                xmlSchemaDocument.Save(xsdSchemaDocument);
            }

            System.Console.WriteLine(string.Format(
                CultureInfo.InvariantCulture, 
                "Xsd schema file was created. Full path to file {0}", 
                new FileInfo(outputFileName).FullName));
        }

        private void ChangeSchemaAttributes(XmlNode xmlElement, XmlNamespaceManager nsman)
        {
            XmlNode node = xmlElement.SelectSingleNode("//xs:schema", nsman);

            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.Name == "xmlns")
                    attribute.Value = attribute.Value.Replace("@test.suite.template@", outputFileName);

                if (attribute.Name == "targetNamespace")
                    attribute.Value = attribute.Value.Replace("@test.suite.template@", outputFileName);
            }
        }

        /// <summary>
        /// Create child nodes from business action data object.
        /// </summary>
        /// <param name="xmlDocument">Represents an xml document</param>
        /// <param name="xmlNode">Parent node</param>
        /// <param name="nameSpace">Namespace of xsd document</param>
        /// <param name="businessActionData">Business action data</param>
        private void CreateChildNodes(XmlDocument xmlDocument, XmlNode xmlNode, string nameSpace, BusinessActionData businessActionData)
        {
            foreach (BusinessActionEntry entry in businessActionData.Actions)
            {
                XmlNode newNode = xmlDocument.CreateNode(XmlNodeType.Element, "xs", "element", nameSpace);

                // add attribute name
                XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("name");
                xmlAttribute.Value = entry.ActionId;
                newNode.Attributes.Append(xmlAttribute);

                // add attribute minOccurs
                xmlAttribute = xmlDocument.CreateAttribute("minOccurs");
                xmlAttribute.Value = "0";
                newNode.Attributes.Append(xmlAttribute);

                if (entry.ActionParameters.Count > 0)
                {
                    CreateComplexTypeNode(xmlDocument, newNode, nameSpace, entry.ActionParameters);
                }
                else
                {
                    // add attribute type
                    xmlAttribute = xmlDocument.CreateAttribute("type");
                    xmlAttribute.Value = "xs:string";
                    newNode.Attributes.Append(xmlAttribute);
                }

                // append child node
                xmlNode.AppendChild(newNode);
            }
        }
        
        /// <summary>
        /// Generate xsd file
        /// </summary>
        /// <param name="businessActionData">Business action data</param>
        /// <returns>Return xsd schema</returns>
        private XmlDocument GenerateXsdSchema(BusinessActionData businessActionData)
        {
            using (Stream stream = File.Open(XsdTemplateFileName, FileMode.Open))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(stream);

                XmlElement xmlElement = xmlDocument.DocumentElement;

                if (xmlElement == null)
                {
                    throw new XmlException("Xsd file is invalid");
                }

                string nameSpace = xmlElement.GetAttribute("xmlns:xs");
                XmlNamespaceManager nsman = new XmlNamespaceManager(xmlDocument.NameTable);
                nsman.AddNamespace("xs", nameSpace);

                // change xmlns name and targetNamespace
                ChangeSchemaAttributes(xmlElement, nsman);

                XmlNode xmlNode = xmlElement.SelectSingleNode("//xs:element[@name='steps']/xs:complexType/xs:sequence", nsman);
                CreateChildNodes(xmlDocument, xmlNode, nameSpace, businessActionData);

                return xmlDocument;
            }
        }

        /// <summary>
        /// Create complext type node and attribute child nodes
        /// </summary>
        /// <param name="xmlDocument">Represents an xml document</param>
        /// <param name="xmlNode">Parent node</param>
        /// <param name="nameSpace">Namespace of xsd document</param>
        /// <param name="parameters">Action parameters</param>
        private static void CreateComplexTypeNode(XmlDocument xmlDocument, XmlNode xmlNode, string nameSpace, IEnumerable<BusinessActionParameters> parameters)
        {
            // create element complextype
            XmlNode complexTypeNode = xmlDocument.CreateNode(XmlNodeType.Element, "xs", "complexType", nameSpace);

            // go through all business action paramters and add elements to xsd schema
            foreach (BusinessActionParameters parameter in parameters)
            {
                // create element attribute
                XmlNode attributeNode = xmlDocument.CreateNode(XmlNodeType.Element, "xs", "attribute", nameSpace);
                
                // append attribute name
                XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("name");
                xmlAttribute.Value = parameter.ParameterName;
                attributeNode.Attributes.Append(xmlAttribute);

                // append attribute type
                xmlAttribute = xmlDocument.CreateAttribute("type");
                xmlAttribute.Value = string.Format(CultureInfo.InvariantCulture, "xs:{0}", parameter.ParameterType ?? "string");
                attributeNode.Attributes.Append(xmlAttribute);

                complexTypeNode.AppendChild(attributeNode);
            }

            xmlNode.AppendChild(complexTypeNode);
        }

        /// <summary>
        /// Parse business action xml document to object
        /// </summary>
        /// <param name="businessActionsXmlFileName">file name of business action</param>
        /// <returns>Parsed xml document as BusinessActionData object</returns>
        private static BusinessActionData ParseXmlToObject(string businessActionsXmlFileName)
        {
            BusinessActionData businessActionData;

            using (Stream xmlStream = File.OpenRead(businessActionsXmlFileName))
            {
                IBusinessActionXmlParser businessActionXmlParser = new BusinessActionsXmlParser(xmlStream);
                businessActionData = businessActionXmlParser.Parse();
            }

            return businessActionData;
        }

        private string businessActionsXmlFileName;
        private readonly IConsoleCommand nextCommandInChain;
        private string outputFileName;
        private const string AccipioActionsXsdFileName = @"..\..\..\Data\Samples\AccipioActions.xsd";
        private const string XsdTemplateFileName = @"..\..\..\Data\Samples\TestSuiteTemplate.xsd";
    }
}