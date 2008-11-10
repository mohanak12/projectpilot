#region

using System.Collections.Generic;
using System.Globalization;

#endregion

namespace ProjectPilot.TestFramework
{
	public class HtmlTestCodeGenerator : ITestCodeGenerator
	{
		public HtmlTestCodeGenerator(ICodeWriter writer)
		{
			this.writer = writer;
		}



		public void Generate(TestSpecs testSpecs)
		{
			WriteLine("<body>");
			Dictionary<string, TestCase> testCases = testSpecs.TestCases;
			foreach (string testCaseName in testCases.Keys)
			{
				WriteLine("<h1>{0}</h1>", testCaseName);
				TestCase testCase = testSpecs.GetTestCase(testCaseName);
				foreach (TestAction testAction in testCase.TestActions)
				{
					WriteLine(testAction);
				}
			}
			WriteLine("</body>");
		}



		private void WriteLine(TestAction testAction)
		{
			if (testAction.HasParameters)
			{
				foreach (TestActionParameter actionParameter in testAction.ActionParameters)
				{
					WriteLine("<i>{0}</i>{1}<br />", testAction.ActionName, actionParameter.ParameterValue);
				}
			}
			else
			{
				WriteLine("<i>{0}</i><br />", testAction.ActionName);
			}
		}



		private void WriteLine(string line)
		{
			writer.WriteLine(line);
		}



		private void WriteLine
			(string format,
			 params object[] args)
		{
			writer.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
		}



		private readonly ICodeWriter writer;
	}
}