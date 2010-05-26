using System;
using System.DirectoryServices;
using System.Globalization;
using System.IO;

namespace Flubu.Tasks.Iis
{
    public class DeleteApplicationPoolTask : TaskBase
    {
        public DeleteApplicationPoolTask(string applicationPoolName, bool failIfNotExist)
        {
            ApplicationPoolName = applicationPoolName;
            FailIfNotExist = failIfNotExist;
        }

        public override string TaskDescription
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "Delete application pool '{0}'.",
                    ApplicationPoolName);
            }
        }

        public static void Execute(
            IScriptExecutionEnvironment environment,
            string applicationPoolName,
            bool failIfNotExist)
        {
            DeleteApplicationPoolTask task = new DeleteApplicationPoolTask(applicationPoolName, failIfNotExist);
            task.Execute(environment);
        }

        protected override void DoExecute(IScriptExecutionEnvironment environment)
        {
            string version = GetLocalIisVersionTask.GetIisVersion(environment, false);
            int major = GetLocalIisVersionTask.GetMajorVersion(version);
            if (major < 6 || major == 0)
            {
                environment.LogMessage("IIS does not support application pools.");
                return;
            }

            const string AppPoolsRootName = @"IIS://localhost/W3SVC/AppPools";

            using (DirectoryEntry parent = new DirectoryEntry(AppPoolsRootName))
            {
                DirectoryEntry applicationPoolEntry = null;

                try
                {
                    try
                    {
                        applicationPoolEntry = parent.Children.Find(ApplicationPoolName, "IIsApplicationPool");
                        applicationPoolEntry.DeleteTree();
                        applicationPoolEntry.CommitChanges();
                    }
                    catch (DirectoryNotFoundException)
                    {
                        // application pool already exists
                        if (FailIfNotExist)
                        {
                            throw new RunnerFailedException(
                                String.Format(
                                    CultureInfo.InvariantCulture,
                                    "Application '{0}' does not exist.",
                                    ApplicationPoolName));
                        }

                        environment.LogMessage(
                            String.Format(
                                CultureInfo.InvariantCulture,
                                "Application pool '{0}' does not exist, doing nothing.",
                                ApplicationPoolName));
                        return;
                    }
                }
                finally
                {
                    if (applicationPoolEntry != null)
                        applicationPoolEntry.Dispose();
                }

                parent.CommitChanges();
            }
        }

        private string ApplicationPoolName { get; set; }

        private bool FailIfNotExist { get; set; }
    }
}