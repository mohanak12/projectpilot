using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
                TestCase testCase = testSuite.GetTestCase(testCaseName);
                // add test case description
                WriteLine("                .AddDescription(\"{0}\")", testCase.TestCaseDescription);
                // add test case tags
                foreach (string tag in testCase.GetTestCaseTags)
                {
                    WriteLine("                .AddTag(\"{0}\")", tag);
                }

                WriteLine(string.Empty);
                // add test case actions
                IList<TestAction> testActions = testCase.TestActions;
                int counter = 1;
                foreach (TestAction testAction in testActions)
                {
                    AddActionDescription(testAction, businessActionData);
                    StringBuilder line = new StringBuilder();
                    line.AppendFormat(CultureInfo.InvariantCulture, "                .{0}(", testAction.ActionName);

                    if (testAction.HasParameters)
                    {
                        string commaSeparator = string.Empty;

                        foreach (TestActionParameter actionParameters in testAction.ActionParameters)
                        {
                            line.AppendFormat(
                                CultureInfo.InvariantCulture, 
                                "{1}\"{0}\"", 
                                actionParameters.ParameterValue,
                                commaSeparator);

                            commaSeparator = ", ";
                        }
                    }

                    line.Append(")");

                    if (counter == testActions.Count)
                        line.Append(";");

                    WriteLine(line.ToString());

                    counter++;
                }

                WriteLine("        }");
                WriteLine("    }");
                WriteLine(string.Empty);
            }

            WriteLine("}");
        }

        /// <summary>
        /// Adds the action description (Action Comment).
        /// </summary>
        /// <param name="testAction">The test action <see cref="TestAction"/></param>
        /// <param name="businessActionData">The business action data <see cref="businessActionData"/></param>
        private void AddActionDescription(TestAction testAction, BusinessActionData businessActionData)
        {
            const string Line = "                // {0}";
            string description = businessActionData.GetAction(testAction.ActionName).Description;
            string lineFormat = string.Format(CultureInfo.InvariantCulture, Line, description);
            if (testAction.HasParameters)
            {
                string[] parameters = new string[testAction.ActionParametersCount];

                for (int i = 0; i < testAction.ActionParameters.Count; i++)
                {
                    TestActionParameter testActionParameter = testAction.ActionParameters[i];
                    parameters[i] = testActionParameter.ParameterValue;
                }

                WriteLine(lineFormat, parameters);
            }
            else
            {
                WriteLine(lineFormat);
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