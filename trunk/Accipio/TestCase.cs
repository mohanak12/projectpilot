﻿using System.Collections.Generic;

namespace Accipio
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
        /// Adds test case tag to list of tags
        /// </summary>
        /// <param name="tag">Test case tag name</param>
        public void AddTestCaseTag(string tag)
        {
            tags.Add(tag);
        }

        /// <summary>
        /// Gets test case tags
        /// </summary>
        public IList<string> GetTestCaseTags
        {
            get { return tags; }
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
        /// <param name="testAction">See <see cref="testAction"/></param>
        public void AddTestAction(TestAction testAction)
        {
            testActions.Add(testAction);
        }

        /// <summary>
        /// Returns specified test action.
        /// </summary>
        /// <param name="testActionName">Test action Name <see cref="TestAction.ActionName"/></param>
        /// <returns>See <see cref="TestAction"/></returns>
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
        /// Gets or sets the test case description.
        /// </summary>
        /// <value>The test case description.</value>
        public string TestCaseDescription { get; set; }

        /// <summary>
        /// Collection of Test actions.
        /// </summary>
        private readonly List<TestAction> testActions = new List<TestAction>();
        private readonly List<string> tags = new List<string>();
        private readonly string testCaseName;
        private readonly string testCaseCategory = "Smoke";
    }
}