using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ProjectPilot.TestFramework
{
    public class CSharpTestCodeGenerator : ITestCodeGenerator
    {
        public CSharpTestCodeGenerator(string fileName)
        {
            this.fileName = fileName;
        }

        public void Generate(TestSpecs testSpecs)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                Dictionary<string, TestCase> testCases = testSpecs.TestCases;
                ICollection<string> testCasesKeys = testCases.Keys;
                foreach (string testCaseName in testCasesKeys)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "void {0}()", testCaseName);
                    sb.AppendLine();
                    sb.AppendLine("{");
                    sb.AppendLine("    Tester tester = new Tester();");
                    TestCase testCase = testSpecs.GetTestCase(testCaseName);
                    List<TestAction> testActions = testCase.TestActions;
                    foreach (TestAction testAction in testActions)
                    {
                        if (testAction.HasParameters)
                        {
                            sb.AppendFormat(CultureInfo.InvariantCulture, "    tester.{0}(\"{1}\");", testAction.ActionName,
                                            testAction.Parameter);
                        }
                        else
                        {
                            sb.AppendFormat(CultureInfo.InvariantCulture, "    tester.{0}();", testAction.ActionName);
                        }
                        sb.AppendLine();
                    }
                    sb.AppendLine("}");
                    sb.AppendLine();
                }
                sw.Write(sb.ToString());
            }
        }

        private readonly string fileName;
    }
}