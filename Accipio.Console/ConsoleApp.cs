using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
            AddCommand(new TestSuiteSchemaGeneratorCommand());
            AddCommand(new TestCodeGeneratorCommand());
            AddCommand(new ReportConverter());
            AddCommand(new HtmlTestReportGeneratorCommand());
        }

        public static string AccipioDirectoryPath
        {
            get
            {
                Uri uri = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                string localPath = uri.LocalPath;
                string dir = Path.GetFullPath(Path.GetDirectoryName(localPath));
                return dir;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public int Process()
        {
            ShowBanner();

            try
            {
                if (args.Length == 0)
                {
                    ShowHelp();
                    return 0;
                }

                string commandName = args[0];

                if (commands.ContainsKey(commandName))
                {
                    IConsoleCommand command = commands[commandName];

                    List<string> remainingArgs = new List<string>(args);
                    remainingArgs.RemoveAt(0);

                    return command.Execute(remainingArgs);
                }
                else
                {
                    throw new ArgumentException(
                        String.Format(
                            CultureInfo.InvariantCulture,
                            "Unknown command: '{0}'",
                            commandName));
                }
            }
            catch (OptionException ex)
            {
                System.Console.Out.WriteLine("ERROR: {0}", ex);
                ShowHelp();
            }
            catch (ArgumentException ex)
            {
                System.Console.Out.WriteLine("ERROR: {0}", ex);
                ShowHelp();
            }
            catch (Exception ex)
            {
                System.Console.Out.WriteLine("ERROR: {0}", ex);
            }

            return 1;
        }

        private void AddCommand(IConsoleCommand command)
        {
            commands.Add(command.CommandName, command);
        }

        private void ShowHelp()
        {
            System.Console.WriteLine("USAGE: Accipio.Console <command> <options>");
            System.Console.WriteLine("-----------------------");
            System.Console.WriteLine("LIST OF COMMANDS:");
            System.Console.WriteLine();

            foreach (IConsoleCommand command in commands.Values)
            {
                System.Console.WriteLine("Command '{0}': {1}", command.CommandName, command.CommandDescription);
                System.Console.WriteLine();
                System.Console.WriteLine("Options for '{0}':", command.CommandName);
                System.Console.WriteLine();
                command.ShowHelp();
                System.Console.WriteLine("-----------------------");
            }
        }

        private static void ShowBanner()
        {
            FileVersionInfo version = 
                FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            System.Console.WriteLine("Accipio.Console v{0}", version.FileVersion);
            System.Console.WriteLine();
        }

        private readonly string[] args;
        private SortedDictionary<string, IConsoleCommand> commands = new SortedDictionary<string, IConsoleCommand>();
    }
}