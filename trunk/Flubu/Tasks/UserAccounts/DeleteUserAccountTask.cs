using System;
using System.DirectoryServices;
using System.Runtime.InteropServices;

namespace Flubu.Tasks.UserAccounts
{
    public class DeleteUserAccountTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Delete user account '{0}'", 
                    userName);
            }
        }

        public DeleteUserAccountTask (string userName)
        {
            this.userName = userName;
        }

        public static void Execute (IScriptExecutionEnvironment environment, string userName)
        {
            DeleteUserAccountTask task = new DeleteUserAccountTask (userName);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            using (DirectoryEntry computerDirectoryEntry = new DirectoryEntry ("WinNT://" + Environment.MachineName + ",computer"))
            {
                try
                {
                    using (DirectoryEntry userEntry = computerDirectoryEntry.Children.Find (userName, "user"))
                    {
                        computerDirectoryEntry.Children.Remove (userEntry);
                    }
                }
                catch (COMException ex)
                {
                    if (ex.ErrorCode == -2147022675)
                    {
                        // if the user does not exist
                        environment.Logger.Log(
                            "User '{0}' does not exist, nothing to do.",
                            userName);
                        return;
                    }
                    else
                        throw;
                }
            }
        }

        private string userName;
    }
}
