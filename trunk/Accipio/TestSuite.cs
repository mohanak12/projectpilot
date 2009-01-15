using System;
using System.Collections.Generic;
using System.Globalization;

namespace Accipio
{
    public class TestSuite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.
        /// </summary>
        /// <param name="testSuiteName">Name of the test suite.</param>
        public TestSuite(string testSuiteName)
        {
            TestSuiteName = testSuiteName;
        }

        /// <summary>
        /// Gets or sets the business action data.
        /// </summary>
        /// <value>The <see cref="BusinessActionData"/></value>
        public BusinessActionData BusinessActionData { get; set; }

        /// <summary>
        /// Gets or sets the value which indicates how many concurrent test cases can run in this test suite.
        /// </summary>
        /// <remarks>In order for this setting to take effect, 
        /// you have to set the <see cref="IsParallelizable"/> to <c>true</c>.</remarks>
        /// <value>The degree of parallelism.</value>
        public int DegreeOfParallelism
        {
            get { return degreeOfParallelism; }
            set { degreeOfParallelism = value; }
        }

        /// <summary>
        /// Gets or sets the test suite description.
        /// Used for class description.
        /// </summary>
        /// <value>The description of the suite.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the test cases in this test suite can run in parallel.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the test cases can run in parallel; otherwise, <c>false</c>.
        /// </value>
        /// <seealso cref="DegreeOfParallelism"/>
        public bool IsParallelizable
        {
            get { return isParallelizable; }
            set { isParallelizable = value; }
        }

        /// <summary>
        /// Gets or sets the namespace of the Test suite.
        /// </summary>
        /// <value>The name of the namespace.</value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets total count of test cases in collection.
        /// </summary>
        public int TestCasesCount
        {
            get { return testCases.Count; }
        }

        /// <summary>
        /// Gets or sets the name of the TestRunner.
        /// </summary>
        /// <value>TestRunner name.</value>
        public string TestRunnerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the test suite.
        /// </summary>
        /// <value>The name of the test suite.</value>
        public string TestSuiteName { get; set; }

        /// <summary>
        /// Adds test case to collection.
        /// </summary>
        /// <param name="testCase">See <see cref="testCase"/></param>
        /// <exception cref="ArgumentException">Test case with the same name has already been added to the test suite.</exception>
        public void AddTestCase(TestCase testCase)
        {
            if (testCases.ContainsKey(testCase.TestCaseName))
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Test case with the name '{0}' has already been added to the test suite '{1}'",
                        testCase.TestCaseName,
                        this.TestSuiteName));

            testCases.Add(testCase.TestCaseName, testCase);
        }

        /// <summary>
        /// Returns specified test case.
        /// </summary>
        /// <param name="testCaseName">See <see cref="TestCase.TestCaseName"/></param>
        /// <returns>Specified Test case <see cref="TestCase"/></returns>
        /// <exception cref="KeyNotFoundException">The test case cannot be found.</exception>
        public TestCase GetTestCase(string testCaseName)
        {
            if (false == testCases.ContainsKey(testCaseName))
                throw new KeyNotFoundException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Test case '{0}' cannot be found in the test suite '{1}'",
                        testCaseName,
                        this.TestSuiteName));

            return testCases[testCaseName];
        }

        /// <summary>
        /// Returns a list of test cases in this test suite.
        /// </summary>
        /// <returns>A list of test cases.</returns>
        public IList<TestCase> ListTestCases()
        {
            return testCases.Values;
        }

        private int degreeOfParallelism = 10;
        private bool isParallelizable;
        private readonly SortedList<string, TestCase> testCases = new SortedList<string, TestCase>();
    }
}