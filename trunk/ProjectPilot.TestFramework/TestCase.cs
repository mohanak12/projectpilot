using System.Collections.Generic;

namespace ProjectPilot.TestFramework
{
    public class TestCase
    {
        /// <summary>
        /// Initializes a new instance of the TestCase class with specified category.
        /// </summary>
        /// <param name="testCaseName">Name of the test case.</param>
        /// <param name="testCaseCategory">Test category (Smoke, Regression, Functional, ...)</param>
        public TestCase(string testCaseName, string testCaseCategory)
        {
            this.testCaseName = testCaseName;
            this.testCaseCategory = testCaseCategory;
        }

        /// <summary>
        /// Initializes a new instance of the TestCase class.
        /// </summary>
        /// <param name="testCaseName">Test case name</param>
        public TestCase(string testCaseName)
        {
            this.testCaseName = testCaseName;
        }

        /// <summary>
        /// Gets list of collected test actions. <see cref="TestAction"/>
        /// </summary>
        public IList<TestAction> TestActions
        {
            get { return testActions; }
        }

        /// <summary>
        /// Gets the total number of collected test actions.
        /// </summary>
        public int TestActionsCount
        {
            get { return testActions.Count; }
        }

        /// <summary>
        /// Gets testcase category.
        /// </summary>
        public string TestCaseCategory
        {
            get { return testCaseCategory; }
        }

        /// <summary>
        /// Gets the TestCase name.
        /// </summary>
        public string TestCaseName
        {
            get
            {
                return testCaseName;
            }
        }

        /// <summary>
        /// Adds test action to Test case.
        /// </summary>
        /// <param name="testAction"><see cref="testAction"/></param>
        public void AddTestAction(TestAction testAction)
        {
            testActions.Add(testAction);
        }

        /// <summary>
        /// Returns specified test action.
        /// </summary>
        /// <param name="testActionName">Test action Name <see cref="TestAction.ActionName"/></param>
        /// <returns><see cref="TestAction"/></returns>
        public TestAction GetTestAction(string testActionName)
        {
            foreach (TestAction testAction in testActions)
            {
                string actionName = testAction.ActionName;
                if (actionName.Equals(testActionName))
                {
                    return testAction;
                }
            }

            return null;
        }

        /// <summary>
        /// Collection of Test actions.
        /// </summary>
        private readonly List<TestAction> testActions = new List<TestAction>();

        private readonly string testCaseName;
        private readonly string testCaseCategory = "Smoke";
    }
}