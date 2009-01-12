using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using NDesk.Options;

namespace Accipio.Console
{
    public class ConsoleApp
    {
        public ConsoleApp(string[] args)
        {
            this.args = args;
            consoleCommandChain = new TestSuiteSchemaGeneratorCommand(
                new TestCodeGeneratorCommand(null));

            Uri uri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            string localPath = uri.LocalPath;
            OutputDirectory = Path.GetDirectoryName(localPath);

            options = new OptionSet
                          {
                              { "o=", "Specifies the output {directory} where all generated files will be stored", 
                                  v => OutputDirectory = v },
                          };
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public int Process()
        {
            try
            {
                ShowBanner();

                List<string> unhandledArguments = options.Parse(args);
                if (unhandledArguments.Count > 0)
                {
                    for (int i=0;i<unhandledArguments.Count;i++)
                    {
                        args[i] = unhandledArguments[i];
                    }
                }

                IConsoleCommand commandToExecute = consoleCommandChain.ParseArguments(args);

                if (commandToExecute == null)
                    throw new ArgumentException("Unknown command.");
                commandToExecute.AccipioDirectory = OutputDirectory;
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

        private void ShowHelp()
        {
            System.Console.WriteLine(string.Empty);
            System.Console.WriteLine("Usage: Accipio.Console.exe [options] [target] [arguments]");
            System.Console.WriteLine(string.Empty);
            System.Console.WriteLine("Options:");
            options.WriteOptionDescriptions(System.Console.Out);
            System.Console.WriteLine(string.Empty);
            System.Console.WriteLine("Target:");
            System.Console.WriteLine("  baschema                   Create acceptance test suite schema");
            System.Console.WriteLine("  codegen                    Create acceptance test code");
            System.Console.WriteLine(string.Empty);
            System.Console.WriteLine("Arguments:");
            System.Console.WriteLine("  [baschema]");
            System.Console.WriteLine("  <instance>.xml             Name of a xml file to infer xsd schema from");
            System.Console.WriteLine("  <schema_namespace>         Xsd schema namespace to be used for validating <instance>.xml");
            System.Console.WriteLine("  [codegen]");
            System.Console.WriteLine("  <instance>.xml             Name of a xml file to infer xsd schema from");
            System.Console.WriteLine("  <schema>.xsd               Name of a schema to be used for validating <test_suite_file>.xml");
            System.Console.WriteLine("  <test_suite_file>.xml      Name of a test suite file.");
            System.Console.WriteLine("                             Multipile file arguments of type <test_suite_file>.xml may be provided.");
            System.Console.WriteLine(string.Empty);
            System.Console.WriteLine("Example:");
            System.Console.WriteLine("  Accipio.Console.exe baschema <instance>.xml <schema_namespace>");
            System.Console.WriteLine("  Accipio.Console.exe codegen <instance>.xml <schema>.xsd <test_suite_file>.xml [...]");
            System.Console.WriteLine("  Accipio.Console.exe -o=C:\\temp\\ codegen <instance>.xml <schema>.xsd <test_suite_file>.xml [...]");
            System.Console.WriteLine(string.Empty);
        }

        private static void ShowBanner()
        {
            FileVersionInfo version = 
                FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            System.Console.WriteLine("Accipio.Console v{0}", version.FileVersion);
            System.Console.WriteLine();
        }

        private string[] args;
        private readonly IConsoleCommand consoleCommandChain;
        private readonly OptionSet options;

        public string OutputDirectory { get; set; }
    }
}