#region

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
			//TODO: possible more than one XML file with test cases.
			testCasesXML = new FileInfo(args[3]);

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
			Stream streamTestCases =
				AccipioHelper.GetXmlFileContent(testCasesXML.FullName);
			XmlTestSpecsParser testSpecsParser = new XmlTestSpecsParser(streamTestCases);
			testSpecs = testSpecsParser.Parse();
		}

		public void Process()
		{
			//generate c# code
			//TODO: add business actions descriptions to generated code
			string generatedFileName = testCasesXML.FullName.Substring(0, testCasesXML.FullName.Length - 4);
			string csharpCodeFile = generatedFileName + ".cs";
			System.Console.WriteLine("Creating '{0}'", csharpCodeFile);
			using (ICodeWriter writer = new FileCodeWriter(csharpCodeFile))
			{
				ITestCodeGenerator cSharpCode = new CSharpTestCodeGenerator(writer);
				cSharpCode.Generate(testSpecs);
			}

			//generate html test specifications
			//TODO: add business actions descriptions to test spec
			string htmlCodeFile = generatedFileName + ".html";
			System.Console.WriteLine("Creating '{0}'", htmlCodeFile);
			using (ICodeWriter writer = new FileCodeWriter(htmlCodeFile))
			{
				ITestCodeGenerator htmlTestSpecs = new HtmlTestCodeGenerator(writer);
				htmlTestSpecs.Generate(testSpecs);
			}
		}

		private TestSpecs testSpecs;
		//private BusinessActions businessActions;

		private FileInfo businessActionXML;
		private FileInfo businessActionXSD;
		//TODO: possible more than one XML file with test cases.
		private FileInfo testCasesXML;
	}
}