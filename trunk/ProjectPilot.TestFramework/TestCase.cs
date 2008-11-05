using System.Collections.Generic;

namespace ProjectPilot.TestFramework
{
    public class TestCase
    {
        public TestCase(string testCaseName)
        {
            this.testCaseName = testCaseName;
        }

        public IList<TestAction> TestActions
        {
            get { return testActions; }
        }

        /// <summary>
        /// Total number of collected test actions.
        /// </summary>
        /// 
        public int TestActionsCount
        {
            get { return testActions.Count; }
        }

        /// <summary>
        /// Gets or sets the TestCase name.
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
        /// 
        public void AddTestAction(TestAction testAction)
        {
            testActions.Add(testAction);
        }

        /// <summary>
        /// Returns specified test action.
        /// </summary>
        /// <param name="testActionName">Test action Name <see cref="TestAction.ActionName"/></param>
        /// <returns><see cref="TestAction"/></returns>
        /// 
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
        private List<TestAction> testActions = new List<TestAction>();

        private string testCaseName;
    }
}