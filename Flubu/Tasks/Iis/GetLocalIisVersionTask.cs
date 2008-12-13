using System;
using System.Collections.Generic;
using System.Text;
using Flubu.Tasks.Registry;
using Microsoft.Win32;

namespace Flubu.Tasks.Iis
{
    public class GetLocalIisVersionTask : TaskBase
    {
        public override string TaskDescription
        {
            get { return "Get local IIS version"; }
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

        public GetLocalIisVersionTask ()
        {            
        }

        public static void ExecuteTask(IScriptExecutionEnvironment environment)
        {
            GetLocalIisVersionTask task = new GetLocalIisVersionTask();
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            GetRegistryValueTask innerTask = new GetRegistryValueTask (
                Microsoft.Win32.Registry.LocalMachine,
                @"SOFTWARE\Microsoft\InetStp",
                "MajorVersion",
                "IIS/MajorVersion");
            innerTask.Execute (environment);

            innerTask = new GetRegistryValueTask (
                Microsoft.Win32.Registry.LocalMachine,
                @"SOFTWARE\Microsoft\InetStp",
                "MinorVersion",
                "IIS/MinorVersion");
            innerTask.Execute (environment);

            environment.LogMessage(
                "Local IIS has version {0}.{1}",
                environment.GetConfigurationSettingValue ("IIS/MajorVersion"),
                environment.GetConfigurationSettingValue ("IIS/MinorVersion"));
        }
    }
}
