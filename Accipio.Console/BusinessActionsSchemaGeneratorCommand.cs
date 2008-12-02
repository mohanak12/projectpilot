using System;

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
            string businessActionsXmlFileName = args[0];
            XmlValidationHelper helper = new XmlValidationHelper();
            helper.ValidateXmlDocument(businessActionsXmlFileName, @"..\..\..\Data\Samples\AccipioActions.xsd");

            //validating XML with schema file (automatic)

            //parsing XML file and retrieving TestActions, parameters etc
            //XmlTestSuiteParser parser = new XmlTestSuiteParser(content);
            //TestSuite testSpecs = parser.Parse();

            //using (ICodeWriter writer = new FileCodeWriter(OutputFile))
            //{
            //    ITestCodeGenerator cSharpCode = new CSharpTestCodeGenerator(writer);
            //    cSharpCode.Generate(testSpecs);
            //}
            //generating XSD file which contains these actions

            //return true;

            return this;
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        public void ProcessCommand()
        {
            throw new NotImplementedException();
        }

        private readonly IConsoleCommand nextCommandInChain;
    }
}