#region

using System.Collections.Generic;

#endregion

namespace ProjectPilot.TestFramework
{
    public class TestAction
    {
        /// <summary>
        /// Initializes a new instance of the TestAction class.
        /// </summary>
        /// <param name="actionName">Name of the action</param>
        public TestAction(string actionName)
        {
            this.actionName = actionName;
        }

        /// <summary>
        /// Initializes a new instance of the TestAction class.
        /// </summary>
        /// <param name="actionName">Name of the action</param>
        /// <param name="testActionParameter">Action parameters <see cref="testActionParameter"/></param>
        public TestAction (
            string actionName,
            TestActionParameter testActionParameter)
        {
            this.actionName = actionName;
            AddActionParameter(testActionParameter);
        }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <example><c>selectProject</c> in <selectProject>Mobi-Info</selectProject></example>
        public string ActionName
        {
            get { return actionName; }
        }

        /// <summary>
        /// Add action parameter.
        /// </summary>
        /// <param name="testActionParameter">See <see cref="TestActionParameter"/></param>
        /// <example>url=http://asd.html</example>
        /// <example>name=parameter1</example>
        public void AddActionParameter(TestActionParameter testActionParameter)
        {
            actionParameters.Add(testActionParameter);
        }

        /// <summary>
        /// Gets number of action parameters in test action.
        /// </summary>
        public int ActionParametersCount
        {
            get { return actionParameters.Count; }
        }

        /// <summary>
        /// Gets collection of action parameters.
        /// </summary>
        public IList<TestActionParameter> ActionParameters
        {
            get { return actionParameters; }
        }

        /// <summary>
        /// Gets <c>value</c> of specified <c>key</c>.
        /// </summary>
        /// <example>
        /// If TestAction has TestActionParameter url="http://asd.aspx",
        /// Return value GetParameterKeyValue("url") is "http://asd.aspx".
        /// </example>
        /// <param name="parameterKey">XML attribute key</param>
        /// <returns>XML attribute value</returns>
        public string GetParameterKeyValue(string parameterKey)
        {
            foreach (TestActionParameter actionParameter in actionParameters)
            {
                if (actionParameter.ParameterKey.Equals(parameterKey))
                {
                    return actionParameter.ParameterValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a value indicating whether the test actiona has parameters.
        /// </summary>
        public bool HasParameters
        {
            get { return actionParameters.Count > 0 ? true : false; }
        }

        private readonly string actionName;

        /// <summary>
        /// Collection of test action parameters.
        /// </summary>
        private readonly List<TestActionParameter> actionParameters = new List<TestActionParameter>();
    }
}