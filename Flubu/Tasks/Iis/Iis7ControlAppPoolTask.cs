using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Web.Administration;

namespace Flubu.Tasks.Iis
{
    public class Iis7ControlAppPoolTask : TaskBase, IControlAppPoolTask
    {
        public string ApplicationPoolName
        {
            get { return applicationPoolName; }
            set { applicationPoolName = value; }
        }

        public ControlApplicationPoolAction Action
        {
            get { return action; }
            set { action = value; }
        }

        public bool FailIfNotExist
        {
            get { return failIfNotExist; }
            set { failIfNotExist = value; }
        }

        public override string TaskDescription
        {
            get
            {
                return string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "{1} application pool '{0}'.",
                    applicationPoolName,
                    action);
            }
        }

        protected override void DoExecute(IScriptExecutionEnvironment environment)
        {
            ServerManager serverManager = new ServerManager();
            ApplicationPoolCollection applicationPoolCollection = serverManager.ApplicationPools;

            foreach (ApplicationPool applicationPool in applicationPoolCollection)
            {
                if (applicationPool.Name == ApplicationPoolName)
                {
                    switch (action)
                    {
                        case ControlApplicationPoolAction.Start:
                            RunWithRetries(x => applicationPool.Start(), 3);
                            break;
                        case ControlApplicationPoolAction.Stop:
                            applicationPool.Stop();
                            break;
                        case ControlApplicationPoolAction.Recycle:
                            applicationPool.Recycle();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    serverManager.CommitChanges();

                    environment.LogMessage(
                        String.Format(
                            System.Globalization.CultureInfo.InvariantCulture,
                            "Application pool '{0}' has been deleted.",
                            ApplicationPoolName));

                    return;
                }
            }

            string message = String.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "Application pool '{0}' does not exist.",
                applicationPoolName);

            if (failIfNotExist)
                throw new RunnerFailedException(message);
            else
                environment.LogMessage(message);
        }

        private static void RunWithRetries(Action<int> action, int retries)
        {
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    action(0);
                    break;
                }
                catch (COMException)
                {
                    if (i == retries-1)
                        throw;
                    Thread.Sleep(1000);
                }
            }
        }

        private string applicationPoolName;
        private ControlApplicationPoolAction action;
        private bool failIfNotExist;
    }
}