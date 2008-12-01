#region

using System;
using System.Collections.Generic;
using System.IO;

#endregion

namespace Accipio.Console
{
    /// <summary>
    /// Generating testing source code and documentation.
    /// </summary>
    public class TestCodeGeneratorCommand : IConsoleCommand
    {
        public TestCodeGeneratorCommand(IConsoleCommand nextCommandInChain)
        {
            this.nextCommandInChain = nextCommandInChain;
        }

        public IConsoleCommand ParseArguments(string[] args)
        {
            if (args.Length < 1
                || 0 != String.Compare(args[0], "codegen", StringComparison.OrdinalIgnoreCase))
            {
                if (nextCommandInChain != null)
                    return nextCommandInChain.ParseArguments(args);
                return null;
            }

            testSuitesFileNames = new List<string>();

            businessActionsXmlFileName = args[1];
            businessActionsSchemaFileName = args[2];
            for (int i = 3; i < args.Length; i++)
                testSuitesFileNames.Add(args[i]);

            //TODO: uncoment when BusinessActions ready
            //parse business actions
            //XmlTestSuiteParser parser =
            //    new XmlTestSuiteParser(XmlValidationHelper.GetXmlFileContent(businessActionXML.FullName));
            //BusinessActions businessActions = parser.Parse();

            //validate XML content
            XmlValidationHelper helper = new XmlValidationHelper();
            helper.ValidateXmlDocument(businessActionsXmlFileName, businessActionsSchemaFileName);

            return this;
        }

        public void ProcessCommand()
        {
            foreach (string testSuiteFileName in testSuitesFileNames)
            {
                using (XmlTestSuiteParser testSuiteParser = new XmlTestSuiteParser(testSuiteFileName))
                {
                    TestSuite parsedTestSuite = testSuiteParser.Parse();

                    // generate c# code
                    // TODO: add business actions descriptions to generated code
                    string codeFileName = Path.ChangeExtension(testSuiteFileName, ".cs");

                    using (ICodeWriter writer = new FileCodeWriter(codeFileName))
                    {
                        ITestCodeGenerator codeGenerator = new CSharpTestCodeGenerator(writer);
                        codeGenerator.Generate(parsedTestSuite);
                    }

                    // generate html test specifications
                    // TODO: add business actions descriptions to test spec
                    string htmlFileName = Path.ChangeExtension(testSuiteFileName, ".html");
                    System.Console.WriteLine("Creating '{0}'", htmlFileName);

                    using (ICodeWriter writer = new FileCodeWriter(htmlFileName))
                    {
                        ITestCodeGenerator htmlTestSpecs = new HtmlTestCodeGenerator(writer);
                        htmlTestSpecs.Generate(parsedTestSuite);
                    }
                }
            }
        }

        private string businessActionsSchemaFileName;
        private string businessActionsXmlFileName;
        private readonly IConsoleCommand nextCommandInChain;
        private List<string> testSuitesFileNames;
        //private BusinessActions businessActions;
    }
}