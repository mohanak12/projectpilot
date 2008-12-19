using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Accipio.Console
{
    /// <summary>
    /// Generate test suite XML schema file.
    /// </summary>
    public class TestSuiteSchemaGeneratorCommand : IConsoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the TestSuiteSchemaGeneratorCommand class.
        /// </summary>
        /// <param name="nextCommandInChain">Application arguments</param>
        public TestSuiteSchemaGeneratorCommand(IConsoleCommand nextCommandInChain)
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

            if (args.Length < 3)
                throw new ArgumentException("Missing XML namespace for the new test suite schema.");
            testSuiteSchemaNamespace = args[2];

            FileInfo fileInfo = new FileInfo(businessActionsXmlFileName);

            // check if file exists
            if (!fileInfo.Exists)
                throw new System.IO.IOException(
                    string.Format(
                    CultureInfo.InvariantCulture, 
                    "File {0} does not exist.", 
                    businessActionsXmlFileName));

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
            using (Stream xsdSchemaDocument = File.Open(outputFileName, FileMode.Create))
            {
                xmlSchemaDocument.Save(xsdSchemaDocument);
            }

            System.Console.WriteLine(string.Format(
                CultureInfo.InvariantCulture, 
                "XSD schema file was created. Full path to file: '{0}'", 
                new FileInfo(outputFileName).FullName));
        }

        /// <summary>
        /// Adds XML nodes for the specified parameters.
        /// </summary>
        /// <param name="testSuiteSchemaDocument"><see cref="XmlDocument"/> which contains the test suite template XSD.</param>
        /// <param name="xmlNode">Parent node</param>
        /// <param name="parameters">Action parameters</param>
        private void AddBusinessActionParameters(
            XmlDocument testSuiteSchemaDocument, 
            XmlNode xmlNode, 
            IList<BusinessActionParameters> parameters)
        {
            // create element complextype
            XmlNode complexTypeNode = testSuiteSchemaDocument.CreateNode(
                XmlNodeType.Element, 
                "xs", 
                "complexType", 
                XmlNsXs);

            if (parameters.Count == 0)
            {
                // add restriction to action withput parameters to disable text node

                // add complextContent node
                XmlNode complexContentNode = testSuiteSchemaDocument.CreateNode(
                XmlNodeType.Element,
                "xs",
                "complexContent",
                XmlNsXs);

                // add restriction node
                XmlNode restrictionNode = testSuiteSchemaDocument.CreateNode(
                XmlNodeType.Element,
                "xs",
                "restriction",
                XmlNsXs);

                // add base attribute to restriction node
                // append attribute name
                XmlAttribute xmlAttribute = testSuiteSchemaDocument.CreateAttribute("base");
                xmlAttribute.Value = "xs:anyType";
                restrictionNode.Attributes.Append(xmlAttribute);

                // append restrictionNode to complexContentNode
                complexContentNode.AppendChild(restrictionNode);

                // append complexContentNode to complexTypeNode
                complexTypeNode.AppendChild(complexContentNode);
            }
            else
            {
                // go through all business action paramters and add elements to xsd schema
                foreach (BusinessActionParameters parameter in parameters)
                {
                    // create element attribute
                    XmlNode attributeNode = testSuiteSchemaDocument.CreateNode(
                        XmlNodeType.Element,
                        "xs",
                        "attribute",
                        XmlNsXs);

                    // append attribute name
                    XmlAttribute xmlAttribute = testSuiteSchemaDocument.CreateAttribute("name");
                    xmlAttribute.Value = parameter.ParameterName;
                    attributeNode.Attributes.Append(xmlAttribute);

                    // append attribute type
                    xmlAttribute = testSuiteSchemaDocument.CreateAttribute("type");
                    xmlAttribute.Value = string.Format(CultureInfo.InvariantCulture, "xs:{0}", parameter.ParameterType ?? "string");
                    attributeNode.Attributes.Append(xmlAttribute);

                    xmlAttribute = testSuiteSchemaDocument.CreateAttribute("use");
                    xmlAttribute.Value = "required";
                    attributeNode.Attributes.Append(xmlAttribute);

                    complexTypeNode.AppendChild(attributeNode);
                }
            }

            xmlNode.AppendChild(complexTypeNode);
        }

        /// <summary>
        /// Fills the test suite template XSD with the required business actions nodes.
        /// </summary>
        /// <param name="testSuiteSchemaDocument"><see cref="XmlDocument"/> which contains the test suite template XSD.</param>
        /// <param name="testActionsParentNode">Parent node where the test actions should be filled.</param>
        /// <param name="businessActionData">Business action data</param>
        private void FillTestSuiteSchemaTemplate(
            XmlDocument testSuiteSchemaDocument, 
            XmlNode testActionsParentNode, 
            BusinessActionData businessActionData)
        {
            foreach (BusinessActionEntry entry in businessActionData.Actions)
            {
                XmlNode newNode = testSuiteSchemaDocument.CreateNode(
                    XmlNodeType.Element, 
                    "xs", 
                    "element", 
                    XmlNsXs);

                // add attribute name
                XmlAttribute xmlAttribute = testSuiteSchemaDocument.CreateAttribute("name");
                xmlAttribute.Value = entry.ActionId;
                newNode.Attributes.Append(xmlAttribute);

                // add attribute minOccurs
                xmlAttribute = testSuiteSchemaDocument.CreateAttribute("minOccurs");
                xmlAttribute.Value = "1";
                newNode.Attributes.Append(xmlAttribute);

                //if (entry.ActionParameters.Count == 0)
                //{
                    // add complex node with restriction
                //}
                //else if (entry.ActionParameters.Count > 0)
                //{
                    AddBusinessActionParameters(testSuiteSchemaDocument, newNode, entry.ActionParameters);
                //}

                // append child node
                testActionsParentNode.AppendChild(newNode);
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

                //string nameSpace = xmlElement.GetAttribute("xmlns:xs");
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                namespaceManager.AddNamespace("xs", XmlNsXs);

                // change xmlns name and targetNamespace
                SetXmlDocumentNamespaces(xmlDocument);

                XmlNode xmlNode = xmlElement.SelectSingleNode(
                    "//xs:element[@name='steps']/xs:complexType/xs:sequence/xs:choice",
                    namespaceManager);
                FillTestSuiteSchemaTemplate(xmlDocument, xmlNode, businessActionData);

                return xmlDocument;
            }
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

        private void SetXmlDocumentNamespaces(XmlDocument xmlDocument)
        {
            XmlAttribute targetNamespaceAtt = xmlDocument.CreateAttribute("targetNamespace");
            targetNamespaceAtt.Value = testSuiteSchemaNamespace;
            xmlDocument.DocumentElement.Attributes.Append(targetNamespaceAtt);
        }

        private const string AccipioActionsXsdFileName = @"AccipioActions.xsd";
        private string businessActionsXmlFileName;
        private readonly IConsoleCommand nextCommandInChain;
        private string outputFileName;
        private string testSuiteSchemaNamespace;
        private const string XsdTemplateFileName = @"TestSuiteTemplate.xsd";
        private const string XmlNsXs = "http://www.w3.org/2001/XMLSchema";
    }
}