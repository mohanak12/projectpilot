using System.Collections.Generic;
using System.Globalization;

namespace Accipio
{
    public class CSharpTestCodeGenerator : ITestCodeGenerator
    {
        public CSharpTestCodeGenerator(ICodeWriter writer)
        {
            this.writer = writer;
        }

        public void Generate(TestSuite testSuite)
        {
            BusinessActionData businessActionData = testSuite.BusinessActionData;
            Dictionary<string, TestCase> testCases = testSuite.TestCases;
            ICollection<string> testCasesKeys = testCases.Keys;
            WriteLine("/// <summary>{0}</summary>", testSuite.Description);
            WriteLine("public class {0}TestSuite", testSuite.Id);
            WriteLine("{");
            foreach (string testCaseName in testCasesKeys)
            {
                WriteLine("    /// <summary>{0}</summary>", testCases[testCaseName].TestCaseDescription);
                WriteLine("    [Test]");
                WriteLine("    [Category(\"{0}\")]", testCases[testCaseName].TestCaseCategory);
                WriteLine("    public void {0}()", testCaseName);
                WriteLine("    {");
                WriteLine("        using ({0}TestRunner runner = new {0}TestRunner())", testSuite.Runner);
                WriteLine("        {");
                WriteLine("            runner");

                WriteLine(string.Empty);
                TestCase testCase = testSuite.GetTestCase(testCaseName);
                IList<TestAction> testActions = testCase.TestActions;
                foreach (TestAction testAction in testActions)
                {
                    WriteLine("                /// <summary>{0}</summary>", businessActionData.GetAction(testAction.ActionName).Description);
                    if (testAction.HasParameters)
                    {
                        foreach (TestActionParameter actionParameters in testAction.ActionParameters)
                        {
                            WriteLine("                .{0}(\"{1}\");", testAction.ActionName, actionParameters.ParameterValue);
                        }
                    }
                    else
                    {
                        WriteLine("                .{0}();", testAction.ActionName);
                    }
                }

                WriteLine("        }");
                WriteLine("    }");
            }

            WriteLine("}");
            WriteLine(string.Empty);
        }

        private void WriteLine(string line)
        {
            writer.WriteLine(line);
        }

        private void WriteLine(string format, params object[] args)
        {
            writer.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        private readonly ICodeWriter writer;
    }
}