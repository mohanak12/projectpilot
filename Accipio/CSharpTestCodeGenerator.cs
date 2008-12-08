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
                int counter = 1;
                foreach (TestAction testAction in testActions)
                {
                    WriteLine("                // {0}", businessActionData.GetAction(testAction.ActionName).Description);
                    string line = string.Format(CultureInfo.InvariantCulture, "                .{0}(", testAction.ActionName);

                    if (testAction.HasParameters)
                    {
                        foreach (TestActionParameter actionParameters in testAction.ActionParameters)
                        {
                            line += string.Format(CultureInfo.InvariantCulture, "\"{0}\", ", actionParameters.ParameterValue);
                        }

                        line = line.Remove(line.LastIndexOf(','));
                    }

                    if (counter == testActions.Count)
                    {
                        WriteLine(string.Format(CultureInfo.InvariantCulture, "{0});", line));   
                    }
                    else
                    {
                        WriteLine(string.Format(CultureInfo.InvariantCulture, "{0})", line));    
                    }

                    counter++;
                }

                WriteLine("        }");
                WriteLine("    }");
                WriteLine(string.Empty);
            }

            WriteLine("}");
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