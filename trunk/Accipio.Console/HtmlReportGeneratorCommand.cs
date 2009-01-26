using System;
using System.Globalization;
using System.IO;

namespace Accipio.Console
{
    public class HtmlReportGeneratorCommand : IConsoleCommand
    {
        /// <summary>
        /// Initializes a new instance of the HtmlReportGeneratorCommand class.
        /// </summary>
        /// <param name="nextCommandInChain">Application arguments</param>
        public HtmlReportGeneratorCommand(IConsoleCommand nextCommandInChain)
        {
            this.nextCommandInChain = nextCommandInChain;
        }

        /// <summary>
        /// Gets output file name
        /// </summary>
        public string OutputFile
        {
            get { return testReportOutputFileName; }
        }

        public string AccipioDirectory { get; set; }

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
                || 0 != String.Compare(args[0], "report", StringComparison.OrdinalIgnoreCase))
            {
                if (nextCommandInChain != null)
                    return nextCommandInChain.ParseArguments(args);
                return null;
            }

            if (args.Length < 2)
                throw new ArgumentException("Missing accetance test report statistic file name.");
            // set acceptance test report file name
            testReportFileName = args[1];

            if (args.Length < 3)
                throw new ArgumentException("Missing acceptance test report output file name.");
            testReportOutputFileName = args[2];

            FileInfo fileInfo = new FileInfo(testReportFileName);

            // check if file exists
            if (!fileInfo.Exists)
                throw new IOException(
                    string.Format(
                    CultureInfo.InvariantCulture,
                    "File {0} does not exist.",
                    testReportFileName));

            return this;
        }

        /// <summary>
        /// Processes the command.
        /// </summary>
        public void ProcessCommand()
        {
            ReportData reportData;
            using (ReportDataParser parser = new ReportDataParser(testReportFileName))
            {
                // parse report data from xml file
                reportData = parser.Parse();
            }

            System.Console.WriteLine("Creating report '{0}'", testReportOutputFileName);

            // generate html report data
            IHtmlTestReportGenerator htmlTestReportGenerator = new HtmlTestReportGenerator();
            htmlTestReportGenerator.Generate(reportData);
        }

        private readonly IConsoleCommand nextCommandInChain;
        private string testReportFileName;
        private string testReportOutputFileName;
    }
}
