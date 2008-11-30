#region

using System.Collections.Generic;
using System.IO;

#endregion

namespace Accipio.Console
{
    /// <summary>
    /// Generating testing source code and documentation.
    /// </summary>
    public class TestSpecGenerator : IGenerator
    {
        public void Parse(string[] args)
        {
            AccipioHelper.CheckForValidInputArguments(args);

            businessActionXML = new FileInfo(args[1]);
            businessActionXSD = new FileInfo(args[2]);
            for (int i = 3; i < args.Length; i++)
            {
                FileInfo testsXML = new FileInfo(args[i]);
                testCases.Add(testsXML);
            }

            //TODO: uncoment when BusinessActions ready
            //parse business actions
            //XmlTestSpecsParser parser =
            //    new XmlTestSpecsParser(AccipioHelper.GetXmlFileContent(businessActionXML.FullName));
            //BusinessActions businessActions = parser.Parse();

            //validate XML content
            Stream streamBusinessActions =
                AccipioHelper.GetXmlFileContent(businessActionXML.FullName);
            AccipioHelper.ValidateXmlDocument(streamBusinessActions, businessActionXSD.FullName);

            //parse test cases
            foreach (FileInfo testCaseFile in testCases)
            {
                Stream streamTestCases =
                    AccipioHelper.GetXmlFileContent(testCaseFile.FullName);
                XmlTestSpecsParser testSpecsParser = new XmlTestSpecsParser(streamTestCases);
                testSpecs.Add(testCaseFile.Name, testSpecsParser.Parse());
            }
        }

        public void Process()
        {
            foreach (FileInfo testSpecFile in testCases)
            {
                //generate c# code
                //TODO: add business actions descriptions to generated code
                string fileName = testSpecFile.FullName;
                string generatedFileName = fileName.Substring(0, fileName.Length - 4);
                string csharpCodeFile = generatedFileName + ".cs";
                System.Console.WriteLine("Creating '{0}'", csharpCodeFile);
                using (ICodeWriter writer = new FileCodeWriter(csharpCodeFile))
                {
                    ITestCodeGenerator cSharpCode = new CSharpTestCodeGenerator(writer);
                    cSharpCode.Generate(testSpecs[testSpecFile.Name]);
                }

                //generate html test specifications
                //TODO: add business actions descriptions to test spec
                string htmlCodeFile = generatedFileName + ".html";
                System.Console.WriteLine("Creating '{0}'", htmlCodeFile);
                using (ICodeWriter writer = new FileCodeWriter(htmlCodeFile))
                {
                    ITestCodeGenerator htmlTestSpecs = new HtmlTestCodeGenerator(writer);
                    htmlTestSpecs.Generate(testSpecs[testSpecFile.Name]);
                }
            }
        }

        public Dictionary<string, TestSpecs> GetTestSpecs
        {
            get { return testSpecs; }
        }

        private Dictionary<string, TestSpecs> testSpecs;

        //private BusinessActions businessActions;

        private FileInfo businessActionXML;

        private FileInfo businessActionXSD;

        private List<FileInfo> testCases;
    }
}