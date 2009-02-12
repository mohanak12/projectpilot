using System;
using System.Collections.Generic;
using System.Globalization;

namespace Accipio
{
    public class TestCaseStep
    {
        /// <summary>
        /// Initializes a new instance of the TestCaseStep class.
        /// </summary>
        /// <param name="actionName">Name of the action</param>
        public TestCaseStep(string actionName)
        {
            this.actionName = actionName;
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
        /// Gets collection of action parameters.
        /// </summary>
        public IList<TestStepParameter> Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Gets a value indicating whether the test step has parameters.
        /// </summary>
        public bool HasParameters
        {
            get { return parameters.Count > 0 ? true : false; }
        }

        /// <summary>
        /// Add test step parameter.
        /// </summary>
        /// <param name="testStepParameter">See <see cref="TestStepParameter"/></param>
        /// <example>url=http://asd.html</example>
        /// <example>name=parameter1</example>
        public void AddParameter(TestStepParameter testStepParameter)
        {
            parameters.Add(testStepParameter);
        }

        public string ExpandDescriptionWithParameterValues (BusinessActionData businessActionData)
        {
            string description = businessActionData.GetAction(ActionName).Description;
            if (HasParameters)
            {
                List<object> args = new List<object>();
                foreach (TestStepParameter parameter in Parameters)
                    args.Add(parameter.ParameterValue);

                return String.Format(CultureInfo.InvariantCulture, description, args.ToArray());
            }

            return description;
        }

        /// <summary>
        /// Gets <c>value</c> of specified <c>key</c>.
        /// </summary>
        /// <example>
        /// If TestCaseStep has TestStepParameter url="http://asd.aspx",
        /// Return value GetParameterValue("url") is "http://asd.aspx".
        /// </example>
        /// <param name="parameterKey">XML attribute key</param>
        /// <returns>XML attribute value</returns>
        public object GetParameterValue(string parameterKey)
        {
            foreach (TestStepParameter parameter in parameters)
            {
                if (parameter.ParameterName.Equals(parameterKey))
                {
                    return parameter.ParameterValue;
                }
            }

            return null;
        }

        private readonly string actionName;

        /// <summary>
        /// Collection of test action parameters.
        /// </summary>
        private readonly List<TestStepParameter> parameters = new List<TestStepParameter>();
    }
}