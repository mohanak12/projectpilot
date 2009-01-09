using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Accipio.Console
{
    public class TestReportGeneratorCommand : IConsoleCommand
    {
        public TestReportGeneratorCommand(IConsoleCommand nextCommandInChain)
        {
            this.nextCommandInChain = nextCommandInChain;
        }

        public string AccipioDirectory { get; set; }

        public IConsoleCommand ParseArguments(string[] args)
        {
            if (args == null)
                return null;

            int argsLength = args.Length;
            if (argsLength < 1
                || 0 != String.Compare(args[0], "transform", StringComparison.OrdinalIgnoreCase))
            {
                if (nextCommandInChain != null)
                    return nextCommandInChain.ParseArguments(args);
                return null;
            }

            if (argsLength < 2)
                throw new ArgumentException("Missing test report file name.");
            xmlReportFileName = CheckIfFileExists(args[1]);

            if (argsLength < 3)
                throw new ArgumentException("Missing test report output file name.");
            htmlReportFileName = args[2];

            return this;
        }

        public void ProcessCommand()
        {
            XsltSettings xsltSettings = new XsltSettings(true, true);
            XmlDocument xsltDoc = new XmlDocument();
            xsltDoc.Load(Path.Combine(AccipioDirectory, XsltTransformationFileName));

            XmlUrlResolver resolver = new XmlUrlResolver();
            XslCompiledTransform transform = new XslCompiledTransform(true);
            transform.Load(xsltDoc, xsltSettings, resolver);

            using (Stream inputStream = File.Open(xmlReportFileName, FileMode.Open, FileAccess.Read))
            {
                XmlReader reader = XmlReader.Create(inputStream);
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.ConformanceLevel = ConformanceLevel.Auto;
                using (XmlWriter writer = XmlWriter.Create(htmlReportFileName, writerSettings))
                {
                    if (writer != null)
                    {
                        transform.Transform(reader, writer);
                    }
                }
            }
        }

        private static string CheckIfFileExists(string fileName)
        {
            if (!File.Exists(fileName))
                throw new IOException(string.Format(CultureInfo.InvariantCulture, "File {0} does not exists.", fileName));

            return fileName;
        }

        private readonly IConsoleCommand nextCommandInChain;
        private string xmlReportFileName;
        private const string XsltTransformationFileName = @"TestReportTransform.xslt";
        private string htmlReportFileName;
    }
}