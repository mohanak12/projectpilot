#region

using System;
using System.IO;

#endregion

namespace ProjectPilot.TestFramework.Console
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			System.Console.WriteLine("Number of input arguments : {0}", args.Length);
			if (args.Length < 2)
			{
				ShowHelp();
			}
			else
			{
				string fileShema = args[0];
				FileInfo fileInfoShema = new FileInfo(fileShema);
				if (File.Exists(fileInfoShema.FullName))
				{
					if (fileInfoShema.Extension.Equals(".xsd", StringComparison.InvariantCultureIgnoreCase))
					{
						System.Console.WriteLine("Found XML shema : '{0}'", args[0]);
						System.Console.WriteLine();
						for (int i = 1; i <= args.Length - 1; i++)
						{
							string fileXML = args[i];
							FileInfo fileInfoXML = new FileInfo(fileXML);
							if (File.Exists(fileInfoXML.FullName))
							{
								if (fileInfoXML.Extension.Equals(".xml", StringComparison.InvariantCultureIgnoreCase))
								{
									System.Console.WriteLine("Parsing '{0}'", fileInfoXML.FullName);
									string xmlContent = ReadFile(fileInfoXML.FullName);
									XmlTestSpecsParser parser = new XmlTestSpecsParser(xmlContent);
									TestSpecs testSpecs = parser.Parse();

									string fileWithCode = fileInfoXML.FullName.Substring(0, fileInfoXML.FullName.Length - 4) + ".cs";
									System.Console.WriteLine("Creating '{0}'", fileWithCode);
									ICodeWriter writer = new FileCodeWriter(fileWithCode);
									ITestCodeGenerator cSharpCode = new CSharpTestCodeGenerator(writer);
									cSharpCode.Generate(testSpecs);
								}
								else
								{
									System.Console.WriteLine("'{0}' is not XML File!", fileXML);
								}
							}
							else
							{
								System.Console.WriteLine("File '{0}' not found!", fileInfoXML.FullName);
							}
							System.Console.WriteLine();
						}
					}
					else
					{
						System.Console.WriteLine("File with XML Shema not found!");
						ShowHelp();
					}
				}
				else
				{
					System.Console.WriteLine("File '{0}' not found!", fileInfoShema.FullName);
				}
			}
		}

		private static string ReadFile(string sourceFile)
		{
			string content;
			using (StreamReader streamReader = new StreamReader(sourceFile))
			{
				content = streamReader.ReadToEnd();
				streamReader.Close();
				streamReader.Dispose();
			}
			return content;
		}

		private static void ShowHelp()
		{
			System.Console.WriteLine("Usage:");
			System.Console.WriteLine();
			System.Console.WriteLine("{0} XML_Shema XML_File(s)", 1);
			System.Console.WriteLine();
		}
	}
}