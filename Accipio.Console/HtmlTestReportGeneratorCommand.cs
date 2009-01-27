using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Accipio.Reporting;
using NDesk.Options;

namespace Accipio.Console
{
    public class HtmlTestReportGeneratorCommand : IConsoleCommand
    {
        public HtmlTestReportGeneratorCommand()
        {
            options = new OptionSet() 
            {
                { "i|inputdir=", "input {directory} where Accipio test repors are stored (the default is current directory)",
                  (string inputDir) => this.testReportsDir = inputDir },
                { "o|outputdir=", "output {directory} where HTML report files will be generated (the default is current directory)",
                  (string outputDir) => this.outputDir = outputDir },
            };
        }

        public string CommandDescription
        {
            get { return "Generates HTML report from acceptance test logs"; }
        }

        public string CommandName
        {
            get { return "report"; }
        }

        public int Execute(IEnumerable<string> args)
        {
            List<string> unhandledArguments = options.Parse(args);

            if (unhandledArguments.Count > 0)
                throw new ArgumentException("There are some unsupported options.");

            //ReportData reportData;
            //using (ReportDataParser parser = new ReportDataParser(testReportsDir))
            //{
            //    // parse report data from xml file
            //    reportData = parser.Parse();
            //}

            //System.Console.WriteLine("Creating report files in the '{0}' directory", outputDir);

            //// generate html report data
            //IHtmlTestReportGenerator htmlTestReportGenerator = new HtmlTestReportGenerator();
            //htmlTestReportGenerator.Generate(reportData);

            return 0;
        }

        public void ShowHelp()
        {
            options.WriteOptionDescriptions(System.Console.Out);
        }

        private OptionSet options;
        private string outputDir = ".";
        private string testReportsDir = ".";
    }
}
