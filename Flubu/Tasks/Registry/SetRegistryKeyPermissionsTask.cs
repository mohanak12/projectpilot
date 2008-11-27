using System;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;

namespace Flubu.Tasks.Registry
{
    public class SetRegistryKeyPermissionsTask : TaskBase
    {
        public override string TaskDescription
        {
            get
            {
                return String.Format (
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Set registry key '{0}' permissions for user '{1}'.", 
                    registryKeyPath, 
                    identity);
            }
        }
        
        public SetRegistryKeyPermissionsTask (
            RegistryKey rootKey,
            string registryKeyPath,
            string identity,
            RegistryRights registryRights,
            AccessControlType accessControlType)
        {
            this.rootKey = rootKey;
            this.registryKeyPath = registryKeyPath;
            this.identity = identity;
            this.registryRights = registryRights;
            this.accessControlType = accessControlType;
        }

        public static void Execute (
            IScriptExecutionEnvironment environment,
            RegistryKey rootKey,
            string registryKeyPath,
            string identity,
            RegistryRights registryRights,
            AccessControlType accessControlType)
        {
            SetRegistryKeyPermissionsTask task = new SetRegistryKeyPermissionsTask (
                rootKey, 
                registryKeyPath, 
                identity,
                registryRights, 
                accessControlType);
            task.Execute (environment);
        }

        protected override void DoExecute (IScriptExecutionEnvironment environment)
        {
            using (RegistryKey key = rootKey.OpenSubKey (registryKeyPath, true))
            {
                if (key == null)
                    throw new RunnerFailedException (
                        String.Format (
                            System.Globalization.CultureInfo.InvariantCulture,
                            "Registry key '{0}' does not exist.", 
                            registryKeyPath));

                RegistrySecurity security = key.GetAccessControl (AccessControlSections.Access);

                AuthorizationRuleCollection rules = security.GetAccessRules (true, true, typeof(NTAccount));

                RegistryAccessRule accessRule = new RegistryAccessRule (
                    identity, 
                    registryRights,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.InheritOnly,
                    accessControlType);

                security.SetAccessRule (accessRule);

                key.SetAccessControl (security);
            }
        }

        private RegistryKey rootKey;
        private string registryKeyPath;
        private string identity;
        private RegistryRights registryRights;
        private AccessControlType accessControlType;
    }
}
