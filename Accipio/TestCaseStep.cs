﻿#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#endregion

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
        /// Initializes a new instance of the TestCaseStep class.
        /// </summary>
        /// <param name="actionName">Name of the action</param>
        /// <param name="testActionParameter">Action parameters <see cref="testActionParameter"/></param>
        public TestCaseStep (
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

        public string ExpandDescriptionWithParameterValues (BusinessActionData businessActionData)
        {
            string description = businessActionData.GetAction(ActionName).Description;
            if (HasParameters)
            {
                List<string> parameters = new List<string>();
                foreach (TestActionParameter actionParameter in ActionParameters)
                    parameters.Add(actionParameter.ParameterValue);

                return String.Format(CultureInfo.InvariantCulture, description, parameters.ToArray());
            }

            return description;
        }

        public string ExpandTestCaseStepWithParametersForCSharp(BusinessActionData businessActionData)
        {
            string commaSeparator = string.Empty;
            StringBuilder line = new StringBuilder();
            // get business action parameters
            List<BusinessActionParameters> businessActionParameters =
                (List<BusinessActionParameters>)
                businessActionData.GetAction(ActionName).ActionParameters;

            foreach (TestActionParameter parameter in ActionParameters)
            {
                TestActionParameter tempParameter = parameter;

                BusinessActionParameters parameterType =
                    businessActionParameters.Find(parameters => parameters.ParameterName ==
                                                                tempParameter.ParameterKey);
                if (parameterType.ParameterType == "string")
                {
                    line.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{1}\"{0}\"",
                        parameter.ParameterValue,
                        commaSeparator);
                }
                else
                {
                    line.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{1}{0}",
                        parameter.ParameterValue,
                        commaSeparator);
                }

                commaSeparator = ", ";
            }

            return line.ToString();
        }

        /// <summary>
        /// Gets <c>value</c> of specified <c>key</c>.
        /// </summary>
        /// <example>
        /// If TestCaseStep has TestActionParameter url="http://asd.aspx",
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