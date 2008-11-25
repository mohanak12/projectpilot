using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Text;

namespace Flubu.Tasks.EnterpriseServices
{
    public class InstallAssemblyTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Install COM+ assembly '{0}'", 
                    assemblyFileName);
            }
        }

        public InstallAssemblyTask (string assemblyFileName)
        {
            this.assemblyFileName = assemblyFileName;
        }

        public static void Execute(IScriptExecutionEnvironment environment, string assemblyFileName)
        {
            InstallAssemblyTask task = new InstallAssemblyTask (assemblyFileName);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            RegistrationHelper regHelper = new RegistrationHelper ();

            string application = null;
            string tlbFileName = null;
            regHelper.InstallAssembly (
                assemblyFileName, 
                ref application, 
                ref tlbFileName,
                InstallationFlags.FindOrCreateTargetApplication|InstallationFlags.ReportWarningsToConsole);
        }

        private string assemblyFileName;
    }
}
