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

            WriteLine("using MbUnit.Framework;");
            WriteLine(string.Empty);
            WriteLine("namespace {0}", testSuite.Namespace);
            WriteLine("{");
            WriteLine("    /// <summary>");
            WriteLine("    /// {0}", testSuite.Description);
            WriteLine("    /// </summary>");
            WriteLine("    [TestFixture]");
            WriteLine("    public class {0}TestSuite", testSuite.TestSuiteName);
            WriteLine("    {");

            int testCaseCount = 1;
            foreach (TestCase testCase in testSuite.ListTestCases())
            {
                WriteLine("        /// <summary>");
                WriteLine("        /// {0}", testCase.TestCaseDescription);
                WriteLine("        /// </summary>");
                WriteLine("        [Test]");
                AddHeaderTags(testCase);
                if (testSuite.IsParallelizable)
                    WriteLine("        [Parallelizable]");
                WriteLine("        public void {0}()", testCase.TestCaseName);
                WriteLine("        {");
                WriteLine("            using ({0}TestRunner runner = new {0}TestRunner())", testSuite.TestRunnerName);
                WriteLine("            {");
                WriteLine("                runner");
                // add test case description
                WriteLine("                    .SetDescription(\"{0}\")", testCase.TestCaseDescription);
                // add test case tags
                AddTags(testCase);

                WriteLine(string.Empty);
                WriteLine("                runner");
                // add test case actions
                IList<TestAction> testActions = testCase.TestSteps;
                int counter = 1;
                foreach (TestAction testAction in testActions)
                {
                    AddActionDescription(testAction, businessActionData);
                    StringBuilder line = new StringBuilder();
                    line.AppendFormat(CultureInfo.InvariantCulture, "                    .{0}(", testAction.ActionName);

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

                WriteLine("            }");
                WriteLine("        }");

                if (testSuite.TestCasesCount != testCaseCount)
                {
                    WriteLine(string.Empty);
                }

                testCaseCount++;
            }

            // if the test suite support parallelization, add fixture setup code
            // with some parallalism settings
            if (testSuite.IsParallelizable)
            {
                WriteLine(string.Empty);
                WriteLine("        /// <summary>");
                WriteLine("        /// Test fixture setup code.");
                WriteLine("        /// </summary>");
                WriteLine("        [FixtureSetUp]");
                WriteLine("        public void FixtureSetup()");
                WriteLine("        {");
                WriteLine(
                    "            Gallio.Framework.Pattern.PatternTestGlobals.DegreeOfParallelism = {0};",
                    testSuite.DegreeOfParallelism);
                WriteLine("        }");
            }

            WriteLine("    }");
            WriteLine("}");
        }

        /// <summary>
        /// Adds the action description (Action Comment).
        /// </summary>
        /// <param name="testAction">The test action <see cref="TestAction"/></param>
        /// <param name="businessActionData">The business action data <see cref="businessActionData"/></param>
        private void AddActionDescription(TestAction testAction, BusinessActionData businessActionData)
        {
            const string Line = "                    // {0}";
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

        /// <summary>
        /// Adds Unit test attribute <c>Metadata</c> with userstory.
        /// </summary>
        /// <param name="testCase">Test Case <see cref="testCase"/></param>
        private void AddHeaderTags(TestCase testCase)
        {
            foreach (string tag in testCase.Tags)
            {
                WriteLine("        [Metadata(\"UserStory\", \"{0}\")]", tag);
            }
        }

        /// <summary>
        /// Adds tags to each test case.
        /// </summary>
        /// <param name="testCase">Test Case <see cref="testCase"/></param>
        private void AddTags(TestCase testCase)
        {
            int tagCounter = 1;
            foreach (string tag in testCase.Tags)
            {
                if (tagCounter == testCase.Tags.Count)
                {
                    WriteLine("                    .AddTag(\"{0}\");", tag);
                }
                else
                {
                    WriteLine("                    .AddTag(\"{0}\")", tag);
                }

                tagCounter++;
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