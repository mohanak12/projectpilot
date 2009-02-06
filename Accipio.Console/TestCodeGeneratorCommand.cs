#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NDesk.Options;

#endregion

namespace Accipio.Console
{
    /// <summary>
    /// Generating testing source code and documentation.
    /// </summary>
    public class TestCodeGeneratorCommand : IConsoleCommand
    {
        public TestCodeGeneratorCommand()
        {
            options = new OptionSet() 
            {
                { "ba|businessactions=", "Business actions XML {file}",
                  (string file) => this.businessActionsXmlFileName = file },
                { "tx|testsuitesschema=", "Test suites XML schema {file}",
                  (string file) => this.testSuiteXsdFileName = file },
                { "o|outputdir=", "output {directory} where generated C# code will be stored (the default is current directory)",
                  (string outputDir) => this.outputDir = outputDir },
                { "i|inputfile=", "input test suite XML file (can be repeated multiple times)",
                  (string inputfile) => this.testSuitesFileNames.Add(inputfile) },
            };
        }

        public string CommandDescription
        {
            get { return "Generates C# code for running acceptance tests using Gallio"; }
        }

        public string CommandName
        {
            get { return "codegen"; }
        }

        public int Execute(IEnumerable<string> args)
        {
            List<string> unhandledArguments = options.Parse(args);

            if (unhandledArguments.Count > 0)
                throw new ArgumentException("There are some unsupported options.");

            if (String.IsNullOrEmpty(businessActionsXmlFileName))
                throw new ArgumentException("Missing business actions XML file name.");

            if (String.IsNullOrEmpty(testSuiteXsdFileName))
                throw new ArgumentException("Missing test suites XSD file name.");

            // parse business actions
            using (Stream xmlStream = File.OpenRead(businessActionsXmlFileName))
            {
                IBusinessActionXmlParser businessActionXmlParser = new BusinessActionsXmlParser(xmlStream);
                businessActionData = businessActionXmlParser.Parse();
            }

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();

            foreach (string testSuiteFileName in testSuitesFileNames)
            {
                // validate xml with xsd schema
                xmlValidationHelper.ValidateXmlDocument(testSuiteFileName, testSuiteXsdFileName);

                using (XmlTestSuiteParser testSuiteParser = new XmlTestSuiteParser(testSuiteFileName))
                {
                    TestSuite parsedTestSuite = testSuiteParser.Parse();
                    parsedTestSuite.BusinessActionData = businessActionData;

                    // generate c# code
                    string codeFileName = Path.Combine(
                        outputDir, 
                        Path.GetFileName(Path.ChangeExtension(testSuiteFileName, ".cs")));
                    System.Console.WriteLine("Creating '{0}'", codeFileName);

                    using (ICodeWriter writer = new FileCodeWriter(codeFileName))
                    {
                        ITestCodeGenerator codeGenerator = new TestCodeGenerator(writer);
                        codeGenerator.Generate(parsedTestSuite);
                    }

                    // generate html test specifications
                    string htmlFileName = Path.ChangeExtension(testSuiteFileName, ".html");
                    System.Console.WriteLine("Creating '{0}'", htmlFileName);

                    using (ICodeWriter writer = new FileCodeWriter(htmlFileName))
                    {
                        ITestCodeGenerator htmlTestSpecs = new HtmlTestCodeGenerator(writer);
                        htmlTestSpecs.Generate(parsedTestSuite);
                    }
                }
            }

            return 0;
        }

        public void ShowHelp()
        {
            options.WriteOptionDescriptions(System.Console.Out);
        }

        private string businessActionsXmlFileName;
        private readonly OptionSet options;
        private string outputDir = ".";
        private string testSuiteXsdFileName;
        private List<string> testSuitesFileNames = new List<string>();
        private BusinessActionData businessActionData;
    }
}