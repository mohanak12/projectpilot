using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using NDesk.Options;
using System.Diagnostics;

namespace Accipio.Console
{
    // generate business actions XML schema file
    // input: test actions XML file
    // output: test specifications XML schema file
    // what has to be done:
    //   1. validating XML with schema file (automatic)
    //   2. parsing XML file and retrieving TestActions, parameters etc
    //     - check that action ID's and parameter names do not have whitespaces
    //   3. generating XSD file which contains these actions

    //  Generating testing source code and documentation
    // # Inputs: business actions XML file, business actions XML schema, test cases XML file
    // # Outputs:
    //   * C# code files which execute the specified tests
    //   * Test case documentation in HTML format 

    // command line: command arguments
    // genbaxsd 
    // gencode 
    public class ConsoleApp
    {
        public ConsoleApp(string[] args)
        {
            this.args = args;
        }

        public void Process()
        {
            ShowBanner();

            try
            {

            }
            catch (OptionException ex)
            {
                System.Console.Out.WriteLine("ERROR: {0}", ex.Message);
                ShowHelp();
            }
            catch (ArgumentException ex)
            {
                System.Console.Out.WriteLine("ERROR: {0}", ex.Message);
                ShowHelp();
            }
        }

        private static void ShowBanner()
        {
            FileVersionInfo version = 
// ReSharper disable AssignNullToNotNullAttribute
                FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly ().Location);
// ReSharper restore AssignNullToNotNullAttribute
            System.Console.Out.WriteLine ("Accipio.Console v{0}", version.FileVersion);
            System.Console.Out.WriteLine ();
        }

        private static void ShowHelp()
        {
            System.Console.Out.WriteLine ();
            System.Console.WriteLine ("Usage: Accipio.Conole [OPTIONS]");
            System.Console.Out.WriteLine ();
        }

        private readonly string[] args;
    }
}