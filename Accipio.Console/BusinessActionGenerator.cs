using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Accipio.Console
{
    /// <summary>
    /// Generate business actions XML schema file.
    /// </summary>
    public class BusinessActionGenerator : IGenerator
    {
        public void Parse(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Missing file name.");
            }

            // read xml file
            using (Stream stream = AccipioHelper.GetXmlFileContent(args[0]))
            {
                AccipioHelper.ValidateXmlDocument(stream, @"..\..\..\Data\Samples\AccipioActions.xsd");
            }




            //validating XML with schema file (automatic)

            //parsing XML file and retrieving TestActions, parameters etc
            //XmlTestSpecsParser parser = new XmlTestSpecsParser(content);
            //TestSpecs testSpecs = parser.Parse();

            //using (ICodeWriter writer = new FileCodeWriter(OutputFile))
            //{
            //    ITestCodeGenerator cSharpCode = new CSharpTestCodeGenerator(writer);
            //    cSharpCode.Generate(testSpecs);
            //}
            //generating XSD file which contains these actions

            //return true;
        }

        public void Process()
        {
            throw new NotImplementedException();
        }

        public string OutputFile { get; set; }
    }
}