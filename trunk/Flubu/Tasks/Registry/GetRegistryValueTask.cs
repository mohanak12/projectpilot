using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Flubu.Tasks.Registry
{
    public class GetRegistryValueTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return String.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Get registry value '{0}@{1}' to configuration setting '{2}'", 
                    registryKeyPath, 
                    registryValueName, 
                    configurationSettingName);
            }
        }

        public GetRegistryValueTask (
            RegistryKey rootKey,
            string registryKeyPath,
            string registryValueName,
            string configurationSettingName)
        {
            this.rootKey = rootKey;
            this.registryKeyPath = registryKeyPath;
            this.registryValueName = registryValueName;
            this.configurationSettingName = configurationSettingName;
        }

        public static void Execute (
            IScriptExecutionEnvironment environment,
            RegistryKey rootKey,
            string registryKeyPath,
            string registryValueName,
            string configurationSettingName)
        {
            GetRegistryValueTask task = new GetRegistryValueTask (
                rootKey, 
                registryKeyPath, 
                registryValueName,
                configurationSettingName);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            using (RegistryKey key = rootKey.OpenSubKey (registryKeyPath, false))
            {
                if (key == null)
                    throw new TaskFailedException (
                        String.Format (
                            System.Globalization.CultureInfo.InvariantCulture,
                            "Registry key '{0}' does not exist.", 
                            registryKeyPath));

                environment.SetConfigurationSettingValue (
                    configurationSettingName, 
                    Convert.ToString (key.GetValue (registryValueName), System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        private RegistryKey rootKey;
        private string registryKeyPath;
        private string registryValueName;
        private string configurationSettingName;
    }
}
