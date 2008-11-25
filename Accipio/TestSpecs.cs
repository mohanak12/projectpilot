using System.Collections.Generic;
using Accipio;

namespace Accipio
{
    public class TestSpecs
    {
        /// <summary>
        /// Gets total count of test cases in collection.
        /// </summary>
        public int TestCasesCount
        {
            get { return testCases.Count; }
        }

        /// <summary>
        /// Adds test case to collection.
        /// </summary>
        /// <param name="testCase">See <see cref="testCase"/></param>
        public void AddTestCase(TestCase testCase)
        {
            testCases.Add(testCase.TestCaseName, testCase);
        }

        /// <summary>
        /// Returns specified test case.
        /// </summary>
        /// <param name="testCaseName">See <see cref="TestCase.TestCaseName"/></param>
        /// <returns>Specified Test case <see cref="TestCase"/></returns>
        public TestCase GetTestCase(string testCaseName)
        {
            return testCases[testCaseName];
        }

        /// <summary>
        /// Gets collection of test cases. <see cref="TestCase"/>
        /// </summary>
        public Dictionary<string, TestCase> TestCases
        {
            get { return testCases; }
        }

        /// <summary>
        /// List of test cases.
        /// </summary>
        private Dictionary<string, TestCase> testCases = new Dictionary<string, TestCase>();
    }
}