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

        public void Generate(TestSpecs testSpecs)
        {
            Dictionary<string, TestCase> testCases = testSpecs.TestCases;
            ICollection<string> testCasesKeys = testCases.Keys;
            foreach (string testCaseName in testCasesKeys)
            {
                WriteLine("[Test]");
                WriteLine("[Category(\"{0}\")]", testCases[testCaseName].TestCaseCategory);
                WriteLine("public void {0}()", testCaseName);
                WriteLine("{");
                WriteLine("    Tester tester = new Tester();");

                TestCase testCase = testSpecs.GetTestCase(testCaseName);
                IList<TestAction> testActions = testCase.TestActions;
                foreach (TestAction testAction in testActions)
                {
                    if (testAction.HasParameters)
                    {
                        foreach (TestActionParameter actionParameters in testAction.ActionParameters)
                        {
                            WriteLine("    tester.{0}(\"{1}\");", testAction.ActionName, actionParameters.ParameterValue);
                        }
                    }
                    else
                    {
                        WriteLine("    tester.{0}();", testAction.ActionName);
                    }

                    WriteLine(string.Empty);
                }

                WriteLine("}");
                WriteLine(string.Empty);
            }
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