using System.Collections.Generic;

namespace ProjectPilot.TestFramework
{
    public class TestSpecs
    {
        /// <summary>
        /// Gets total count of test cases in colelction.
        /// </summary>
        public int TestCasesCount
        {
            get { return testCases.Count; }
        }

        /// <summary>
        /// Adds test case to collection.
        /// </summary>
        /// <param name="testCase"><see cref="testCase"/></param>
        /// 
        public void AddTestCase(TestCase testCase)
        {
            testCases.Add(testCase.TestCaseName, testCase);
        }

        /// <summary>
        /// Returns specified test case.
        /// </summary>
        /// <param name="testCaseName"><see cref="TestCase.TestCaseName"/></param>
        /// <returns></returns>
        public TestCase GetTestCase(string testCaseName)
        {
            return testCases[testCaseName];
        }

        public Dictionary<string, TestCase> TestCases
        {
            get { return testCases; }
        }

        /// <summary>
        /// List of test cases.
        /// </summary>
        /// 
        private Dictionary<string, TestCase> testCases = new Dictionary<string, TestCase>();
    }
}