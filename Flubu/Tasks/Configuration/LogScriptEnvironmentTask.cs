using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu.Tasks.Configuration
{
    /// <summary>
    /// Logs important enviroment information (machine name, OS version, etc).
    /// </summary>
    public class LogScriptEnvironmentTask : TaskBase
    {
        /// <summary>
        /// Gets the task description.
        /// </summary>
        /// <value>The task description.</value>
        public override string TaskDescription
        {
            get { return "Log script environment"; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is safe to execute in dry run mode.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is safe to execute in dry run mode; otherwise, <c>false</c>.
        /// </value>
        public override bool IsSafeToExecuteInDryRun
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Internal task execution code.
        /// </summary>
        /// <param name="environment">The script execution environment.</param>
        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            // log important environment information
            environment.LogMessage("Machine name: {0}", Environment.MachineName);
            environment.LogMessage ("OS Version: {0}", Environment.OSVersion);
            environment.LogMessage ("User name: {0}", Environment.UserName);
            environment.LogMessage ("User domain name: {0}", Environment.UserDomainName);
            environment.LogMessage ("CLR version: {0}", Environment.Version);
            environment.LogMessage ("Current directory: {0}", Environment.CurrentDirectory);
        }
    }
}