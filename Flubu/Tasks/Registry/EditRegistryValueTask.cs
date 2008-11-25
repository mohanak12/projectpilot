using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Flubu.Tasks.Registry
{
    public class EditRegistryValueTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Set registry value '{0}@{1}' = '{2}'", 
                    registryKeyPath, 
                    registryValueName, 
                    registryValueValue);
            }
        }

        public EditRegistryValueTask (
            RegistryKey rootKey,
            string registryKeyPath,
            string registryValueName,
            object registryValueValue)
        {
            this.rootKey = rootKey;
            this.registryKeyPath = registryKeyPath;
            this.registryValueName = registryValueName;
            this.registryValueValue = registryValueValue;
        }

        public static void Execute (
            IScriptExecutionEnvironment environment,
            RegistryKey rootKey,
            string registryKeyPath,
            string registryValueName,
            object registryValueValue)
        {
            EditRegistryValueTask task = new EditRegistryValueTask (
                rootKey, 
                registryKeyPath, 
                registryValueName,
                registryValueValue);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            using (RegistryKey key = rootKey.OpenSubKey (registryKeyPath, true))
            {
                if (key == null)
                    throw new TaskFailedException (
                        String.Format (
                            System.Globalization.CultureInfo.InvariantCulture,
                            "Registry key '{0}' does not exist.", 
                            registryKeyPath));

                key.SetValue (registryValueName, registryValueValue);
            }
        }

        private RegistryKey rootKey;
        private string registryKeyPath;
        private string registryValueName;
        private object registryValueValue;
    }
}
