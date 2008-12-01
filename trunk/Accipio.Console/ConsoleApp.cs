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
            this.consoleCommandChain = new BusinessActionsSchemaGeneratorCommand(
                new TestCodeGeneratorCommand(null));
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void Process()
        {
            try
            {
                ShowBanner();

                IConsoleCommand commandToExecute = consoleCommandChain.ParseArguments(args);

                if (commandToExecute == null)
                    throw new ArgumentException("Unknown command.");

                commandToExecute.ParseArguments(args);
                commandToExecute.ProcessCommand();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Parsing error... Details: {0}", ex);
                ShowHelp();
            }
        }

        private static void ShowHelp()
        {
            System.Console.WriteLine("Usage: Accipio.Conole [OPTIONS]");
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