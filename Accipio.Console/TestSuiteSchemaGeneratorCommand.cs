using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using NDesk.Options;

namespace Accipio.Console
{
    /// <summary>
    /// Generate test suite XML schema file.
    /// </summary>
    public class TestSuiteSchemaGeneratorCommand : IConsoleCommand
    {
        public TestSuiteSchemaGeneratorCommand()
        {
            options = new OptionSet() 
            {
                { "ba|businessactions=", "Business actions XML {file}",
                  (string inputFile) => this.businessActionsXmlFileName = inputFile },
                { "ns|namespace=", "XML {namespace} to use for the generated XSD file",
                  (string inputFile) => this.testSuiteSchemaNamespace = inputFile },
                { "o|outputdir=", "output {directory} where Accipio test report file will be stored (the default is current directory)",
                  (string outputDir) => this.outputDir = outputDir },
            };
        }

        public string CommandDescription
        {
            get { return "Generates XSD schema file for the specified business actions XML file"; }
        }

        public string CommandName
        {
            get { return "baschema"; }
        }

        public string TestSuiteSchemaFileName
        {
            get { return testSuiteSchemaFileName; }
        }

        public int Execute(IEnumerable<string> args)
        {
            List<string> unhandledArguments = options.Parse(args);

            if (unhandledArguments.Count > 0)
                throw new ArgumentException("There are some unsupported options.");

            if (String.IsNullOrEmpty(businessActionsXmlFileName))
                throw new ArgumentException("Missing business actions XML file name.");

            if (String.IsNullOrEmpty(testSuiteSchemaNamespace))
                throw new ArgumentException("Missing XML namespace for the new test suite schema.");

            // set output file name
            testSuiteSchemaFileName = Path.Combine(outputDir, Path.GetFileName(Path.ChangeExtension(businessActionsXmlFileName, "xsd")));

            // create instance of class XmlValidationHelper
            XmlValidationHelper helper = new XmlValidationHelper();
            // validating XML with schema file
            // xml document must have at least one action element

            helper.ValidateXmlDocument(
                businessActionsXmlFileName, 
                Path.Combine(ConsoleApp.AccipioDirectoryPath, AccipioActionsXsdFileName));

            // parse XML file
            BusinessActionsRepository businessActionsRepository = ParseXmlToObject(businessActionsXmlFileName);

            // generating XSD schema file with business actions validation parameters
            XmlDocument xmlSchemaDocument = GenerateXsdSchema(businessActionsRepository);

            // write xsd schema to file
            AccipioHelper.EnsureDirectoryPathExists(testSuiteSchemaFileName, true);
            using (Stream xsdSchemaDocument = File.Open(testSuiteSchemaFileName, FileMode.Create))
            {
                xmlSchemaDocument.Save(xsdSchemaDocument);
            }

            System.Console.WriteLine(string.Format(
                CultureInfo.InvariantCulture, 
                "XSD schema file '{0}' was created", 
                Path.GetFullPath(testSuiteSchemaFileName)));

            return 0;
        }

        public void ShowHelp()
        {
            options.WriteOptionDescriptions(System.Console.Out);
        }

        /// <summary>
        /// Adds XML nodes for the specified parameters.
        /// </summary>
        /// <param name="businessAction">The business action whose parameters should be added.</param>
        /// <param name="testSuiteSchemaDocument"><see cref="XmlDocument"/> which contains the test suite template XSD.</param>
        /// <param name="xmlNode">Parent node</param>
        private void AddBusinessActionParameters(
            BusinessAction businessAction, 
            XmlDocument testSuiteSchemaDocument, 
            XmlNode xmlNode)
        {
            // create element complextype
            XmlNode complexTypeNode = testSuiteSchemaDocument.CreateNode(
                XmlNodeType.Element, 
                "xs", 
                "complexType", 
                XmlNsXs);

            if (businessAction.ParametersCount == 0)
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
                foreach (BusinessActionParameter parameter in businessAction.EnumerateParameters())
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
                    xmlAttribute.Value = string.Format(CultureInfo.InvariantCulture, "xs:{0}", parameter.ParameterXsdType);
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
        /// <param name="businessActionsRepository">Business action data</param>
        private void FillTestSuiteSchemaTemplate(
            XmlDocument testSuiteSchemaDocument, 
            XmlNode testActionsParentNode, 
            BusinessActionsRepository businessActionsRepository)
        {
            foreach (BusinessAction businessAction in businessActionsRepository.EnumerateActions())
            {
                XmlNode newNode = testSuiteSchemaDocument.CreateNode(
                    XmlNodeType.Element, 
                    "xs", 
                    "element", 
                    XmlNsXs);

                // add attribute name
                XmlAttribute xmlAttribute = testSuiteSchemaDocument.CreateAttribute("name");
                xmlAttribute.Value = businessAction.ActionName;
                newNode.Attributes.Append(xmlAttribute);

                // add attribute minOccurs
                xmlAttribute = testSuiteSchemaDocument.CreateAttribute("minOccurs");
                xmlAttribute.Value = "1";
                newNode.Attributes.Append(xmlAttribute);

                AddBusinessActionParameters(businessAction, testSuiteSchemaDocument, newNode);

                // append child node
                testActionsParentNode.AppendChild(newNode);
            }
        }

        /// <summary>
        /// Generate xsd file
        /// </summary>
        /// <param name="businessActionsRepository">Business action data</param>
        /// <returns>Return xsd schema</returns>
        private XmlDocument GenerateXsdSchema(BusinessActionsRepository businessActionsRepository)
        {
            using (Stream stream = File.Open(
                Path.Combine(ConsoleApp.AccipioDirectoryPath, XsdTemplateFileName), 
                FileMode.Open))
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
                FillTestSuiteSchemaTemplate(xmlDocument, xmlNode, businessActionsRepository);

                return xmlDocument;
            }
        }

        /// <summary>
        /// Parse business action xml document to object
        /// </summary>
        /// <param name="businessActionsXmlFileName">file name of business action</param>
        /// <returns>Parsed xml document as BusinessActionsRepository object</returns>
        private static BusinessActionsRepository ParseXmlToObject(string businessActionsXmlFileName)
        {
            BusinessActionsRepository businessActionsRepository;

            using (Stream xmlStream = File.OpenRead(businessActionsXmlFileName))
            {
                IBusinessActionXmlParser businessActionXmlParser = new BusinessActionsXmlParser(xmlStream);
                businessActionsRepository = businessActionXmlParser.Parse();
            }

            return businessActionsRepository;
        }

        private void SetXmlDocumentNamespaces(XmlDocument xmlDocument)
        {
            XmlAttribute targetNamespaceAtt = xmlDocument.CreateAttribute("targetNamespace");
            targetNamespaceAtt.Value = testSuiteSchemaNamespace;
            xmlDocument.DocumentElement.Attributes.Append(targetNamespaceAtt);
        }

        private readonly OptionSet options;
        private string outputDir = ".";

        private const string AccipioActionsXsdFileName = @"AccipioActions.xsd";
        private string businessActionsXmlFileName;
        private string testSuiteSchemaFileName;
        private string testSuiteSchemaNamespace;
        private const string XsdTemplateFileName = @"TestSuiteTemplate.xsd";
        private const string XmlNsXs = "http://www.w3.org/2001/XMLSchema";
    }
}