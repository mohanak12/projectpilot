﻿using System.Collections.Generic;

namespace Accipio
{
    public class TestCase
    {
        /// <summary>
        /// Initializes a new instance of the TestCase class.
        /// </summary>
        /// <param name="testCaseName">Test case name</param>
        public TestCase(string testCaseName)
        {
            this.testCaseName = testCaseName;
        }

        /// <summary>
        /// Gets test case tags
        /// </summary>
        public IList<string> Tags
        {
            get { return tags; }
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
        /// Gets or sets the test case description.
        /// </summary>
        /// <value>The test case description.</value>
        public string TestCaseDescription { get; set; }

        /// <summary>
        /// Gets a list of test case's steps.
        /// </summary>
        public IList<TestCaseStep> TestSteps
        {
            get { return testSteps; }
        }

        /// <summary>
        /// Adds test action to Test case.
        /// </summary>
        /// <param name="testCaseStep">See <see cref="testAction"/></param>
        public void AddStep(TestCaseStep testCaseStep)
        {
            testSteps.Add(testCaseStep);
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
        /// Returns specified test action.
        /// </summary>
        /// <param name="testActionName">Test action Name <see cref="TestCaseStep.ActionName"/></param>
        /// <returns>See <see cref="TestCaseStep"/></returns>
        public TestCaseStep GetTestAction(string testActionName)
        {
            foreach (TestCaseStep testAction in testSteps)
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
        private readonly List<TestCaseStep> testSteps = new List<TestCaseStep>();
        private readonly List<string> tags = new List<string>();
        private readonly string testCaseName;
    }
}