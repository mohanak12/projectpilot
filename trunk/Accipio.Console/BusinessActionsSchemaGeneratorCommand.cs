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
        public BusinessActionsSchemaGeneratorCommand(IConsoleCommand nextCommandInChain)
        {
            this.nextCommandInChain = nextCommandInChain;
        }

        public string OutputFile { get; set; }

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
            if (args.Length < 1
                || 0 != String.Compare(args[0], "baschema", StringComparison.OrdinalIgnoreCase))
            {
                if (nextCommandInChain != null)
                    return nextCommandInChain.ParseArguments(args);
                return null;
            }

            if (args.Length != 2)
            {
                throw new ArgumentException("Missing business actions XML file name.");
            }

            // read xml file
            string businessActionsXmlFileName = args[1];
            XmlValidationHelper helper = new XmlValidationHelper();
            // validating XML with schema file (automatic)
            helper.ValidateXmlDocument(businessActionsXmlFileName, @"..\..\..\Data\Samples\AccipioActions.xsd");

            // parsing XML file
            ParseXmlToObject(businessActionsXmlFileName);

            return this;
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        public void ProcessCommand()
        {
            // path to xsd schema file
            string xsdSchemaFilePath = @"businessActionValidationSchema.xsd";

            // generating XSD schema file with business actions validation parameters
            XmlDocument xmlSchemaDocument = GenerateXsdSchema();

            // write xsd schema to file
            using (Stream xsdSchemaDocument = File.OpenWrite(xsdSchemaFilePath))
            {
                xmlSchemaDocument.Save(xsdSchemaDocument);
            }
        }

        /// <summary>
        /// Create child nodes from business action data object.
        /// </summary>
        /// <param name="xmlDocument">Represents xml document</param>
        /// <param name="xmlNode">Parent node</param>
        /// <param name="nameSpace">Namespace of xsd document</param>
        private void CreateChildNodes(XmlDocument xmlDocument, XmlNode xmlNode, string nameSpace)
        {
            foreach (BusinessActionEntry entry in businessActionData.Actions)
            {
                XmlNode newNode = xmlDocument.CreateNode(XmlNodeType.Element, "xs", "element", nameSpace);

                // add attribute name
                XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("name");
                xmlAttribute.Value = entry.ActionId;
                newNode.Attributes.Append(xmlAttribute);

                // add attribute type
                xmlAttribute = xmlDocument.CreateAttribute("type");
                xmlAttribute.Value = "xs:string";
                newNode.Attributes.Append(xmlAttribute);

                // add attribute minOccurs
                xmlAttribute = xmlDocument.CreateAttribute("minOccurs");
                xmlAttribute.Value = "0";
                newNode.Attributes.Append(xmlAttribute);

                if (entry.ActionParameters.Count > 0)
                {
                    CreateComplexTypeNode(xmlDocument, newNode, nameSpace, entry.ActionParameters);
                }

                // append child node
                xmlNode.AppendChild(newNode);
            }
        }

        /// <summary>
        /// Generate xsd file
        /// </summary>
        /// <returns>Xsd schema</returns>
        private XmlDocument GenerateXsdSchema()
        {
            string xsdTemplate = @"..\..\..\Data\Samples\TestSuiteTemplate.xsd";

            using (Stream stream = File.Open(xsdTemplate, FileMode.Open))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(stream);

                XmlElement xmlElement = xmlDocument.DocumentElement;

                if (xmlElement == null)
                {
                    throw new XmlException("Xsd template file is invalid");
                }

                string nameSpace = xmlElement.GetAttribute("xmlns:xs");
                XmlNamespaceManager nsman = new XmlNamespaceManager(xmlDocument.NameTable);
                nsman.AddNamespace("xs", nameSpace);

                XmlNode xmlNode = xmlElement.SelectSingleNode("//xs:element[@name='steps']/xs:complexType/xs:sequence", nsman);
                CreateChildNodes(xmlDocument, xmlNode, nameSpace);

                return xmlDocument;
            }
        }

        /// <summary>
        /// Create complext type node and attribute child nodes
        /// </summary>
        /// <param name="xmlDocument">Represents xml document</param>
        /// <param name="xmlNode">Parent node</param>
        /// <param name="nameSpace">Namespace of xsd document</param>
        /// <param name="parameters">Action parameters</param>
        private void CreateComplexTypeNode(XmlDocument xmlDocument, XmlNode xmlNode, string nameSpace, IEnumerable<BusinessActionParameters> parameters)
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
        private void ParseXmlToObject(string businessActionsXmlFileName)
        {
            using (Stream xmlStream = File.OpenRead(businessActionsXmlFileName))
            {
                IBusinessActionXmlParser businessActionXmlParser = new BusinessActionsXmlParser(xmlStream);
                businessActionData = businessActionXmlParser.Parse();
            }
        }

        private BusinessActionData businessActionData;
        private readonly IConsoleCommand nextCommandInChain;
    }
}