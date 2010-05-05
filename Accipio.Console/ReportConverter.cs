using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using NDesk.Options;

namespace Accipio.Console
{
    public class ReportConverter : IConsoleCommand
    {
        public ReportConverter()
        {
            options = new OptionSet() 
            {
                { "i|inputfile=", "XML test report {file}",
                  (string inputFile) => this.testReportFile = inputFile },
                { "o|outputdir=", "output {directory} where Accipio test report file will be stored (the default is current directory)",
                  (string outputDir) => this.outputDir = outputDir },
                { "x|xsltfile=", "the XSLT {file} to use for transformation to Accipio XML report file (the default is 'TestReportTransform.xslt')",
                    (string file) => this.xsltTransformationFileName = file }, 
                { "t|testframework=", "explicitly specify test framework [nunit, gallio](default is 'Gallio')", 
                    (string tstFramework) => this.tstFramework = tstFramework },
            };
        }

        public string CommandDescription
        {
            get { return "Transforms Gallio or NUnit test report to Accipio XML format"; }
        }

        public string CommandName
        {
            get { return "transform"; }
        }

        public int Execute(IEnumerable<string> args)
        {
            List<string> unhandledArguments = options.Parse(args);

            if (unhandledArguments.Count > 0)
                throw new ArgumentException("There are some unsupported options.");

            if (String.IsNullOrEmpty(testReportFile))
                throw new ArgumentException("Missing Gallio report file name");

            XsltSettings xsltSettings = new XsltSettings(true, true);
            XmlDocument xsltDoc = new XmlDocument();
            xsltDoc.Load(
                Path.Combine(
                    ConsoleApp.AccipioDirectoryPath, 
                    xsltTransformationFileName));

            XmlUrlResolver resolver = new XmlUrlResolver();
            XslCompiledTransform transform = new XslCompiledTransform(true);
            transform.Load(xsltDoc, xsltSettings, resolver);

            string outputFileName = ConstructOutputFileName();

            using (Stream inputStream = File.Open(testReportFile, FileMode.Open, FileAccess.Read))
            {
                XmlReader reader = XmlReader.Create(inputStream);
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.Indent = true;
                writerSettings.NewLineOnAttributes = false;
                writerSettings.ConformanceLevel = ConformanceLevel.Auto;

                AccipioHelper.EnsureDirectoryPathExists(outputFileName, true);

                using (XmlWriter writer = XmlWriter.Create(outputFileName, writerSettings))
                    transform.Transform(reader, writer);
            }

            return 0;
        }

        public void ShowHelp()
        {
            options.WriteOptionDescriptions(System.Console.Out);
        }

        private string ConstructOutputFileName()
        {
            string fileName = String.Format(
                CultureInfo.InvariantCulture,
                "AccipioTestLog_{0:yyyy}{0:MM}{0:dd}_{0:HH}{0:mm}{0:ss}.xml",
                GetTestRunEndTime());
            return Path.Combine(
                outputDir,
                fileName);
        }

        private DateTime GetTestRunEndTime()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(testReportFile);
            XmlNamespaceManager xmlNSManager = new XmlNamespaceManager(xmlDocument.NameTable);
            switch (tstFramework.ToLowerInvariant())
            {
                case "nunit":
                    {
                        XmlNode nodeDate = xmlDocument.SelectSingleNode("/test-results/@date", xmlNSManager);
                        XmlNode nodeTime = xmlDocument.SelectSingleNode("/test-results/@time", xmlNSManager);
                        DateTime startTime = DateTime.Parse(String.Format(CultureInfo.InvariantCulture, "{0} {1}", nodeDate.Value, nodeTime.Value), CultureInfo.InvariantCulture);
                        XmlNode nodeDuration = xmlDocument.SelectSingleNode("/test-results/test-suite/@time", xmlNSManager);
                        return startTime.AddSeconds(Convert.ToDouble(nodeDuration.Value, CultureInfo.InvariantCulture));
                    }

                default:
                    {
                        break;
                    }
            }

            xmlNSManager.AddNamespace("g", "http://www.gallio.org/");
            XmlNode testRunNode = xmlDocument.SelectSingleNode("/g:report/g:testPackageRun", xmlNSManager);
            if (testRunNode == null)
                throw new InvalidOperationException("Gallio report file is missing the /report/testPackageRun node.");
            return DateTime.Parse(testRunNode.Attributes["endTime"].Value, CultureInfo.InvariantCulture);
        }

        private string testReportFile;
        private string tstFramework = "Gallio";
        private readonly OptionSet options;
        private string outputDir = ".";
        private string xsltTransformationFileName = @"TestReportTransform.xslt";
    }
}