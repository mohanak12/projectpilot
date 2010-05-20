using System;
using System.ServiceProcess;

namespace Flubu.Tasks.SqlServer
{
    public class EnsureSqlServerIsRunningTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Ensure SQL Server instance '{0}' is running", 
                    machineName);
            }
        }

        public EnsureSqlServerIsRunningTask (string machineName)
        {
            this.machineName = machineName;
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            using (ServiceController serviceController = new ServiceController("MSSQLSERVER", machineName))
            {
                if (serviceController.Status != ServiceControllerStatus.Running)
                    serviceController.Start();
            }
        }

        private readonly string machineName;
    }
}
