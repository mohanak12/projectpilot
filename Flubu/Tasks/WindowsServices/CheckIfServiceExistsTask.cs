using System;
using System.ServiceProcess;

namespace Flubu.Tasks.WindowsServices
{
    public class CheckIfServiceExistsTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return String.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Check if Windows service '{0}' exists.", 
                    serviceName);
            }
        }

        public CheckIfServiceExistsTask (string serviceName, string configurationSetting)
        {
            this.serviceName = serviceName;
            this.configurationSetting = configurationSetting;
        }

        public static void Execute (
            IScriptExecutionEnvironment environment,
            string serviceName, 
            string configurationSetting)
        {
            CheckIfServiceExistsTask task = new CheckIfServiceExistsTask (serviceName, configurationSetting);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            try
            {
                using (ServiceController serviceController = new ServiceController (serviceName))
                {
                    // this should throw an exception if the service does not exist
                    System.Runtime.InteropServices.SafeHandle serviceHandle = serviceController.ServiceHandle;

                    environment.SetConfigurationSettingValue (configurationSetting, "true");
                    environment.Logger.Log(
                        String.Format (
                            System.Globalization.CultureInfo.InvariantCulture,
                            "Windows service '{0}' exists.", 
                            serviceName));
                }
            }
            catch (InvalidOperationException)
            {
                environment.SetConfigurationSettingValue (configurationSetting, "false");
                environment.Logger.Log(
                    String.Format (
                        System.Globalization.CultureInfo.InvariantCulture,
                        "Windows service '{0}' does not exist.", 
                        serviceName));
            }
        }

        private string serviceName;
        private string configurationSetting;
    }
}
