using System;
using System.DirectoryServices;
using System.IO;

namespace Flubu.Tasks.Iis
{
    public class CreateApplicationPoolTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Create application pool '{0}'.", 
                    applicationPoolName);
            }
        }

        public CreateApplicationPoolTask (string applicationPoolName, CreateApplicationPoolMode mode)
        {
            this.applicationPoolName = applicationPoolName;
            this.mode = mode;
        }

        public static void Execute(
            IScriptExecutionEnvironment environment,
            string applicationPoolName, 
            CreateApplicationPoolMode mode)
        {
            CreateApplicationPoolTask task = new CreateApplicationPoolTask (applicationPoolName, mode);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            string appPoolsRootName = @"IIS://localhost/W3SVC/AppPools";

            using (DirectoryEntry parent = new DirectoryEntry (appPoolsRootName))
            {
                DirectoryEntry applicationPoolEntry = null;

                try
                {
                    // first check if the user already exists
                    try
                    {
                        applicationPoolEntry = parent.Children.Find (applicationPoolName, "IIsApplicationPool");

                        // application pool already exists
                        if (mode == CreateApplicationPoolMode.DoNothingIfExists)
                        {
                            environment.Logger.Log(
                                String.Format (
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    "Application pool '{0}' already exists, doing nothing.", 
                                    applicationPoolName));
                            return;
                        }
                        else if (mode == CreateApplicationPoolMode.FailIfAlreadyExists)
                        {
                            throw new RunnerFailedException (
                                String.Format (
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    "Application '{0}' already exists.", 
                                    applicationPoolName));
                        }

                        // otherwise we should update the existing application pool
                    }
                    catch (DirectoryNotFoundException)
                    {
                        // application pool does not exist, go on and add it
                        applicationPoolEntry = parent.Children.Add (applicationPoolName, "IIsApplicationPool");
                    }

                    applicationPoolEntry.CommitChanges ();
                }
                finally
                {
                    if (applicationPoolEntry != null)
                        applicationPoolEntry.Dispose ();
                }
            
                parent.CommitChanges ();
            }
        }

        private readonly string applicationPoolName;
        private CreateApplicationPoolMode mode;
    }
}
