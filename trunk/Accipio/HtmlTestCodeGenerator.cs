#region

using System;
using System.Collections.Generic;
using System.Globalization;

#endregion

namespace Accipio
{
    public class HtmlTestCodeGenerator : ITestCodeGenerator
    {
        public void Generate(TestSuite testSuite)
        {
            throw new NotImplementedException();
        }

        //WriteLine(
        //        @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">");
        //    WriteLine(@"<html xmlns=""http://www.w3.org/1999/xhtml"" >");
        //    WriteLine("<head>");
        //    WriteLine("    <title>Test plan</title>");
        //    WriteLine("</head>");
        //    WriteLine("<body>");
        //    WriteLine("    <h1>{0}</h1>", testSuite.TestSuiteName);
        //    WriteLine("    <p>Description : <i>{0}</i></p>", testSuite.Description);

        //    foreach (TestCase testCase in testSuite.ListTestCases())
        //    {
        //        WriteLine("    <h2>{0}</h2>", testCase.TestCaseName);
        //        WriteLine("    <p>Description : <i>{0}</i></p>", testCase.TestCaseDescription);
        //        AddTestCaseTags(testCase);
        //        WriteLine("    <ol>");
        //        foreach (TestCaseStep testAction in testCase.TestSteps)
        //        {
        //            AddTestStep(testAction, testSuite.BusinessActionsRepository);
        //        }

        //        WriteLine("    </ol>");
        //    }

        //    WriteLine("</body>");
        //    WriteLine("</html>");
        //}

        ///// <summary>
        ///// Adds list of tags under test case name.
        ///// </summary>
        ///// <param name="testCase">See TestCase <see cref="testCase"/></param>
        //private void AddTestCaseTags(TestCase testCase)
        //{
        //    if (testCase.Tags.Count > 0)
        //    {
        //        foreach (string testCaseTag in testCase.Tags)
        //        {
        //            WriteLine("    <p>Tag : {0}</p>", testCaseTag);
        //        }
        //    }
        //}

        //private void AddTestStep(TestCaseStep testCaseStep, BusinessActionsRepository businessActionData)
        //{
        //    const string Line = "        <li>{0}</li>";
        //    string description = businessActionData.GetAction(testCaseStep.ActionName).Description;
        //    string lineFormat = string.Format(CultureInfo.InvariantCulture, Line, description);
        //    if (testCaseStep.HasParameters)
        //    {
        //        string[] parameters = new string[testCaseStep.ParametersCount];

        //        for (int i = 0; i < testCaseStep.Parameters.Count; i++)
        //        {
        //            TestStepParameter testActionParameter = testCaseStep.Parameters[i];
        //            parameters[i] = testActionParameter.ParameterValue;
        //        }

        //        WriteLine(lineFormat, parameters);
        //    }
        //    else
        //    {
        //        WriteLine(lineFormat);
        //    }
        //}
    }
}