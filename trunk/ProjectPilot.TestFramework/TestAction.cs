using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectPilot.TestFramework
{
    public class TestAction
    {
        /// <summary>
        /// Name of the action.
        /// </summary>
        /// <example><c>selectProject</c> in <selectProject>Mobi-Info</selectProject></example>
        /// 
        public string ActionName { get; set; }

        /// <summary>
        /// Actions parameters.
        /// </summary>
        /// <example>
        /// <c>Mobi-Info</c> in <selectProject>Mobi-Info</selectProject>
        /// </example>
        /// 
        public string Parameter { get; set; }

    }
}
