using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Accipio.Console
{
    public class ConsoleApp
    {
        public ConsoleApp(string[] args)
        {
            this.args = args;
        }

        public void Process()
        {
            try
            {
                ShowBanner();

                string generatorCommand = args[0];
                InputArgument inputArgument =
                    (InputArgument) Enum.Parse(typeof (InputArgument), generatorCommand, true);

                switch (inputArgument)
                {
                    case InputArgument.BusinessAction:
                        {
                            // generate business actions XML schema file
                            // input: test actions XML file
                            // output: test specifications XML schema file
                            // what has to be done:
                            //   1. validating XML with schema file (automatic)
                            //   2. parsing XML file and retrieving TestActions, parameters etc
                            //     - check that action ID's and parameter names do not have whitespaces
                            //   3. generating XSD file which contains these actions
                            IGenerator generator = new BusinessActionGenerator();
                            generator.Parse(args);
                            generator.Process();

                            break;
                        }
                    case InputArgument.TestSpec:
                        {
                            //  Generating testing source code and documentation
                            // # Inputs: business actions XML file, business actions XML schema, test cases XML file
                            // # Outputs:
                            //   * C# code files which execute the specified tests
                            //   * Test case documentation in HTML format 
                            IGenerator generator = new TestSpecGenerator();
                            generator.Parse(args);
                            generator.Process();
                            break;
                        }
                    default:
                        {
                            ShowHelp();
                            break;
                        }
                }
            }
            catch (ApplicationException aex)
            {
                System.Console.WriteLine("Parsing error... Details: {0}", aex);
            }
        }

        private static void ShowHelp()
        {
            System.Console.WriteLine("Usage: Accipio.Conole [OPTIONS]");
        }

        private static void ShowBanner()
        {
            FileVersionInfo version = 
// ReSharper disable AssignNullToNotNullAttribute
                FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
// ReSharper restore AssignNullToNotNullAttribute
            System.Console.WriteLine("Accipio.Console v{0}", version.FileVersion);
            System.Console.WriteLine();
        }

        private readonly string[] args;
    }
}