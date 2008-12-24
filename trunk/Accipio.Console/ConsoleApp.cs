using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Accipio.Console
{
    public class ConsoleApp
    {
        public ConsoleApp(string[] args)
        {
            this.args = args;
            this.consoleCommandChain = new TestSuiteSchemaGeneratorCommand(
                new TestCodeGeneratorCommand(null));
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public int Process()
        {
            try
            {
                ShowBanner();

                IConsoleCommand commandToExecute = consoleCommandChain.ParseArguments(args);

                if (commandToExecute == null)
                    throw new ArgumentException("Unknown command.");

                commandToExecute.ProcessCommand();

                return 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Parsing error... Details: {0}", ex);
                ShowHelp();

                // exit with error code
                return -1;
            }
        }

        private static void ShowHelp()
        {
            System.Console.WriteLine(string.Empty);
            System.Console.WriteLine("Usage: Accipio.Console.exe [baschema | codegen] [Arguments [...]]");
            System.Console.WriteLine(string.Empty);
            System.Console.WriteLine("         To create acceptance test suite schema use command baschema.");
            System.Console.WriteLine("         example: Accipio.Console.exe baschema <instance>.xml <schema_namespace>");
            System.Console.WriteLine(string.Empty);
            System.Console.WriteLine("         To create acceptance test code use command codegen.");
            System.Console.WriteLine("         example: Accipio.Console.exe codegen <instance>.xml <schema>.xsd <test_suite_file>.xml [...]");
            System.Console.WriteLine(string.Empty);
            System.Console.WriteLine("baschema arguments:");
            System.Console.WriteLine("          <instance>.xml          Name of a xml file to infer xsd schema from");
            System.Console.WriteLine("          <schema_namespace>      Xsd schema namespace to be used for validating <instance>.xml");
            System.Console.WriteLine("codegen arguments:");
            System.Console.WriteLine("          <instance>.xml          Name of a xml file to infer xsd schema from");
            System.Console.WriteLine("          <schema>.xsd            Name of a schema to be used for validating <test_suite_file>.xml");
            System.Console.WriteLine("          <test_suite_file>.xml   Name of a test suite file.");
            System.Console.WriteLine("          Multipile file arguments of type <test_suite_file>.xml may be provided.");
        }

        private static void ShowBanner()
        {
            FileVersionInfo version = 
                FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            System.Console.WriteLine("Accipio.Console v{0}", version.FileVersion);
            System.Console.WriteLine();
        }

        private readonly string[] args;
        private readonly IConsoleCommand consoleCommandChain;
    }
}