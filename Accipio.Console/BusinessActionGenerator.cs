using System;

namespace Accipio.Console
{
    /// <summary>
    /// Generate business actions XML schema file.
    /// </summary>
    public class BusinessActionGenerator : IGenerator
    {
        public BusinessActionGenerator(string content)
        {
            this.content = content;
        }

        public string Content
        {
            get { return content; }
        }

        public void Parse(string[] args)
        {
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

        private readonly string content;
    }
}