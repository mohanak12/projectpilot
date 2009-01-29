using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using NDesk.Options;

namespace Accipio.Console
{
    public class GallioReportConverter : IConsoleCommand
    {
        public GallioReportConverter()
        {
            options = new OptionSet() 
            {
                { "i|inputfile=", "Gallio XML test report {file}",
                  (string inputFile) => this.gallioTestReportFile = inputFile },
                { "o|outputdir=", "output {directory} where Accipio test report file will be stored (the default is current directory)",
                  (string outputDir) => this.outputDir = outputDir },
            };
        }

        public string CommandDescription
        {
            get { return "Transforms Gallio test report to Accipio XML format"; }
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

            if (String.IsNullOrEmpty(gallioTestReportFile))
                throw new ArgumentException("Missing Gallio report file name");

            XsltSettings xsltSettings = new XsltSettings(true, true);
            XmlDocument xsltDoc = new XmlDocument();
            xsltDoc.Load(
                Path.Combine(
                    ConsoleApp.AccipioDirectoryPath, 
                    XsltTransformationFileName));

            XmlUrlResolver resolver = new XmlUrlResolver();
            XslCompiledTransform transform = new XslCompiledTransform(true);
            transform.Load(xsltDoc, xsltSettings, resolver);

            string outputFileName = ConstructOutputFileName();

            using (Stream inputStream = File.Open(gallioTestReportFile, FileMode.Open, FileAccess.Read))
            {
                XmlReader reader = XmlReader.Create(inputStream);
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.Indent = true;
                writerSettings.NewLineOnAttributes = false;
                writerSettings.ConformanceLevel = ConformanceLevel.Auto;
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
            XmlDocument gallioDoc = new XmlDocument();
            gallioDoc.Load(gallioTestReportFile);

            XmlNamespaceManager xmlNSManager = new XmlNamespaceManager(gallioDoc.NameTable);
            xmlNSManager.AddNamespace("g", "http://www.gallio.org/");

            XmlNode testRunNode = gallioDoc.SelectSingleNode("/g:report/g:testPackageRun", xmlNSManager);
            if (testRunNode == null)
                throw new InvalidOperationException("Gallio report file is missing the /report/testPackageRun node.");
            string testRunEndTimeString = testRunNode.Attributes["endTime"].Value;
            DateTime testRunEndTime = DateTime.Parse(testRunEndTimeString, CultureInfo.InvariantCulture);

            string fileName = String.Format(
                CultureInfo.InvariantCulture,
                "AccipioTestLog_{0:yyyy}{0:MM}{0:dd}_{0:HH}{0:mm}{0:ss}.xml",
                testRunEndTime);
            return Path.Combine(
                outputDir,
                fileName);
        }

        private string gallioTestReportFile;
        private readonly OptionSet options;
        private string outputDir = ".";
        private const string XsltTransformationFileName = @"TestReportTransform.xslt";
    }
}