using System;
using System.IO;

namespace Accipio.Console
{
    /// <summary>
    /// Generate business actions XML schema file.
    /// </summary>
    public class BusinessActionGenerator : IGenerator
    {
        public void Parse(string[] args)
        {
            Stream stream = AccipioHelper.GetXmlFileContent(args[0]);
            //AccipioHelper.ReadFile(args[0]);

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