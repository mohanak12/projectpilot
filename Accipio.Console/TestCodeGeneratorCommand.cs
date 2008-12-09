#region

using System;
using System.Collections.Generic;
using System.Globalization;
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
                || 0 != String.Compare(args[0], "codegen", StringComparison.OrdinalIgnoreCase))
            {
                if (nextCommandInChain != null)
                    return nextCommandInChain.ParseArguments(args);

                return null;
            }

            if (args.Length < 2)
                throw new ArgumentException("Missing business actions XML file name.");

            testSuitesFileNames = new List<string>();

            // business action file
            businessActionsXmlFileName = CheckIfFileExists(args[1]);

            if (args.Length < 3)
                throw new ArgumentException("Missing test suite xsd schema file name.");

            // xsd schema file for validating test suite xml files
            string testSuiteXsdValidationSchema = CheckIfFileExists(args[2]);

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();

            // add file names to list
            for (int i = 3; i < args.Length; i++)
            {
                string testSuiteXmlFile = CheckIfFileExists(args[i]);
                xmlValidationHelper.ValidateXmlDocument(testSuiteXmlFile, testSuiteXsdValidationSchema);
                testSuitesFileNames.Add(testSuiteXmlFile);
            }

            // parse business actions
            using (Stream xmlStream = File.OpenRead(businessActionsXmlFileName))
            {
                IBusinessActionXmlParser businessActionXmlParser = new BusinessActionsXmlParser(xmlStream);
                businessActionData = businessActionXmlParser.Parse();
            }

            return this;
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        public void ProcessCommand()
        {
            foreach (string testSuiteFileName in testSuitesFileNames)
            {
                // validate xml with xsd schema

                using (XmlTestSuiteParser testSuiteParser = new XmlTestSuiteParser(testSuiteFileName))
                {
                    TestSuite parsedTestSuite = testSuiteParser.Parse();
                    parsedTestSuite.BusinessActionData = businessActionData;

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

        private static string CheckIfFileExists(string fileName)
        {
            if (!File.Exists(fileName))
                throw new IOException(string.Format(CultureInfo.InvariantCulture, "File {0} does not exists.", fileName));

            return fileName;
        }

        private string businessActionsXmlFileName;
        private readonly IConsoleCommand nextCommandInChain;
        private List<string> testSuitesFileNames;
        private BusinessActionData businessActionData;
    }
}