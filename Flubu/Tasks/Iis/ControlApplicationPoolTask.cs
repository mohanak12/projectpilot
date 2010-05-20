using System;
using System.DirectoryServices;
using System.IO;

namespace Flubu.Tasks.Iis
{
    public class ControlApplicationPoolTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return string.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "{1} application pool '{0}'.", 
                    applicationPoolName, 
                    action);
            }
        }

        public ControlApplicationPoolTask (
            string applicationPoolName, 
            ControlApplicationPoolAction action,
            bool failIfNotExist)
        {
            this.applicationPoolName = applicationPoolName;
            this.action = action;
            this.failIfNotExist = failIfNotExist;
        }

        public static void Execute(
            IScriptExecutionEnvironment environment,
            string applicationPoolName,
            ControlApplicationPoolAction action,
            bool failIfNotExist)
        {
            ControlApplicationPoolTask task = new ControlApplicationPoolTask (
                applicationPoolName, 
                action,
                failIfNotExist);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            string version = GetLocalIisVersionTask.GetIisVersion(environment, failIfNotExist);
            int major = GetLocalIisVersionTask.GetMajorVersion(version);
            if (major < 6 || major == 0)
            {
                environment.LogMessage("IIS does not support application pools.");
                return;
            }

            const string AppPoolsRootName = @"IIS://localhost/W3SVC/AppPools";

            using (DirectoryEntry parent = new DirectoryEntry (AppPoolsRootName))
            {
                DirectoryEntry applicationPoolEntry = null;

                try
                {
                    // first check if the user already exists
                    try
                    {
                        applicationPoolEntry = parent.Children.Find (applicationPoolName, "IIsApplicationPool");

                        applicationPoolEntry.Invoke (action.ToString ());
                    }
                    catch (DirectoryNotFoundException)
                    {
                        string message = String.Format (
                            System.Globalization.CultureInfo.InvariantCulture,
                            "Application pool '{0}' does not exist.", 
                            applicationPoolName);

                        if (failIfNotExist)
                            throw new RunnerFailedException (message);
                        else
                            environment.LogMessage(message);
                    }
                }
                finally
                {
                    if (applicationPoolEntry != null)
                        applicationPoolEntry.Dispose ();
                }
            }
        }

        private string applicationPoolName;
        private ControlApplicationPoolAction action;
        private bool failIfNotExist;
    }
}
