using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Wmi;

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
            ManagedComputer managedComputer = new ManagedComputer (machineName);
            Service sqlServerService = managedComputer.Services["MSSQLSERVER"];

            if (sqlServerService.ServiceState != ServiceState.Running)
                sqlServerService.Start ();
        }

        private string machineName;
    }
}
