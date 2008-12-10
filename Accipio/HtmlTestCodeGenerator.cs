#region

using System.Collections.Generic;
using System.Globalization;

#endregion

namespace Accipio
{
    public class HtmlTestCodeGenerator : ITestCodeGenerator
    {
        public HtmlTestCodeGenerator(ICodeWriter writer)
        {
            this.writer = writer;
        }

        public void Generate(TestSuite testSuite)
        {
            WriteLine(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">");
            WriteLine(@"<html xmlns=""http://www.w3.org/1999/xhtml"" >");
            WriteLine("<head>");
            WriteLine("    <title>Test plan</title>");
            WriteLine("</head>");
            WriteLine("<body>");
            WriteLine("    <h1>{0}</h1>", testSuite.Id);
            WriteLine("    <p>Description : <i>{0}</i></p>", testSuite.Description);
            Dictionary<string, TestCase> testCases = testSuite.TestCases;
            foreach (KeyValuePair<string, TestCase> keyValuePair in testCases)
            {
                TestCase testCase = keyValuePair.Value;
                WriteLine("    <h2>{0}</h2>", testCase.TestCaseName);
                WriteLine("    <p>Category : <i>{0}</i></p>", testCase.TestCaseCategory);
                WriteLine("    <p>Description : <i>{0}</i></p>", testCase.TestCaseDescription);
                AddTestCaseTags(testCase);
                WriteLine("    <ol>");
                foreach (TestAction testAction in testCase.TestActions)
                {
                    WriteLine(testAction, testSuite.BusinessActionData);
                }

                WriteLine("    </ol>");
            }

            WriteLine("</body>");
            WriteLine("</html>");
        }

        /// <summary>
        /// Adds list of tags under test case name.
        /// </summary>
        /// <param name="testCase">See TestCase <see cref="testCase"/></param>
        private void AddTestCaseTags(TestCase testCase)
        {
            if (testCase.GetTestCaseTags.Count > 0)
            {
                foreach (string testCaseTag in testCase.GetTestCaseTags)
                {
                    WriteLine("    <p>Tag : {0}</p>", testCaseTag);
                }
            }
        }

        private void WriteLine(TestAction testAction, BusinessActionData businessActionData)
        {
            WriteLine("        <li>{0}</li>", businessActionData.GetAction(testAction.ActionName).Description);
        }

        private void WriteLine(string line)
        {
            writer.WriteLine(line);
        }

        private void WriteLine (
            string format,
            params object[] args)
        {
            writer.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        private readonly ICodeWriter writer;
    }
}