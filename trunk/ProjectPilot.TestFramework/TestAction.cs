﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.TestFramework
{
    public class TestAction
    {
        public TestAction(string actionName, string parameter)
        {
            this.actionName = actionName;
            this.parameter = parameter;
        }

        public TestAction(string actionName)
        {
            this.actionName = actionName;
        }

        /// <summary>
        /// Name of the action.
        /// </summary>
        /// <example><c>selectProject</c> in <selectProject>Mobi-Info</selectProject></example>
        public string ActionName
        {
            get { return actionName; }
        }

        public bool HasParameters
        {
            get { return parameter != null ? true : false; }
        }

        /// <summary>
        /// Action's parameter.
        /// </summary>
        /// <example>
        /// <c>Mobi-Info</c> in <selectProject>Mobi-Info</selectProject>
        /// </example>
        public string Parameter
        {
            get { return parameter; }
        }

        private string actionName;

        private string parameter;
    }
}