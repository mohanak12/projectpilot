using System;
using System.Collections.Generic;
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
                { 
                    "o|outputdir=", "output {directory} where HTML report files will be generated (the default is 'Reports' directory)",
                    (string outputDir) => this.settings.OutputDirectory = outputDir 
                },
                { 
                    "p|projectname=", "the {text} to use as a project name in generated reports (the default is 'MyProject')",
                    (string projectName) => this.settings.ProjectName = projectName
                },
                { 
                    "t|templatesdir=", "the {directory} where the report templates are stored (the default is 'Templates' directory)",
                    (string dir) => this.settings.TemplatesDirectory = dir
                },
                { 
                    "c|cssfile=", "the CSS {file} to use for generated report files (the default is 'TestReport.css' directory)",
                    (string file) => this.settings.CssFileName = file
                },
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

            TestRunsDatabase db = new TestRunsDatabase();
            db.LoadDatabase(testReportsDir, @"AccipioTestLog_*.xml");

            HtmlTestReportGenerator generator = new HtmlTestReportGenerator(settings);
            generator.Generate(db);

            return 0;
        }

        public void ShowHelp()
        {
            options.WriteOptionDescriptions(System.Console.Out);
        }

        private OptionSet options;
        private HtmlTestReportGeneratorSettings settings = new HtmlTestReportGeneratorSettings("MyProject");
        private string testReportsDir = ".";
    }
}
