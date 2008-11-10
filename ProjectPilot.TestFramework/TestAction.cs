#region

using System.Collections.Generic;

#endregion

namespace ProjectPilot.TestFramework
{
    public class TestAction
    {
        public TestAction(string actionName)
        {
            this.actionName = actionName;
        }


        public TestAction
            (string actionName,
             TestActionParameter testActionParameter)
        {
            this.actionName = actionName;
            AddActionParameter(testActionParameter);
        }


        /// <summary>
        /// Name of the action.
        /// </summary>
        /// <example><c>selectProject</c> in <selectProject>Mobi-Info</selectProject></example>
        public string ActionName
        {
            get { return actionName; }
        }


        /// <summary>
        /// Add action parameter.
        /// </summary>
        /// <param name="testActionParameter"><see cref="TestActionParameter"/></param>
        /// <example>url=http://asd.html</example>
        /// <example>name=parameter1</example>
        /// 
        public void AddActionParameter(TestActionParameter testActionParameter)
        {
            actionParameters.Add(testActionParameter);
        }


        public int ActionParametersCount
        {
            get { return actionParameters.Count; }
        }

        public IList<TestActionParameter> ActionParameters
        {
            get { return actionParameters; }
        }


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


        public bool HasParameters
        {
            get { return actionParameters.Count > 0 ? true : false; }
        }

        private readonly string actionName;

        private readonly List<TestActionParameter> actionParameters = new List<TestActionParameter>();
    }
}