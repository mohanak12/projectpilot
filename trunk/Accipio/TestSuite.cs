using System.Collections.Generic;

namespace Accipio
{
    public class TestSuite
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
        /// Gets or sets the business action data.
        /// </summary>
        /// <value>The <see cref="BusinessActionData"/></value>
        public BusinessActionData BusinessActionData { get; set; }

        /// <summary>
        /// Gets collection of test cases. <see cref="TestCase"/>
        /// </summary>
        public Dictionary<string, TestCase> TestCases
        {
            get { return testCases; }
        }

        /// <summary>
        /// Gets or sets the test suite id.
        /// </summary>
        /// <value>Unique id.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the TestRunner.
        /// </summary>
        /// <value>TestRunner name.</value>
        public string Runner { get; set; }

        /// <summary>
        /// Gets or sets the test suite description.
        /// Used for class description.
        /// </summary>
        /// <value>The description of the suite.</value>
        public string Description { get; set; }

        /// <summary>
        /// List of test cases.
        /// </summary>
        private readonly Dictionary<string, TestCase> testCases = new Dictionary<string, TestCase>();
    }
}