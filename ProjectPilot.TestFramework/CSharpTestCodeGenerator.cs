using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ProjectPilot.TestFramework
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
                writer.WriteLine("void {0}()", testCaseName);
                writer.WriteLine("{");
                writer.WriteLine("    Tester tester = new Tester();");

                TestCase testCase = testSpecs.GetTestCase(testCaseName);
                IList<TestAction> testActions = testCase.TestActions;
                foreach (TestAction testAction in testActions)
                {
                    if (testAction.HasParameters)
                    {
                        writer.WriteLine("    tester.{0}(\"{1}\");", testAction.ActionName,
                                        testAction.Parameter);
                    }
                    else
                    {
                        writer.WriteLine("    tester.{0}();", testAction.ActionName);
                    }
                    writer.WriteLine(string.Empty);
                }
                writer.WriteLine("}");
                writer.WriteLine(string.Empty);
            }
        }

        private readonly ICodeWriter writer;
    }
}