using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Flubu.Deployment
{
    public class DeploymentModulesRegistry<TRunner>
        where TRunner : DeploymentRunner<TRunner>
    {
        public DeploymentModulesRegistry(DeploymentRunner<TRunner> runner)
        {
            this.runner = runner;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "deploymentModule")]
        public DeploymentModule<TRunner> AddModule(DeploymentModule<TRunner> deploymentModule)
        {
            throw new NotImplementedException();
        }

        public WebApplicationDeploymentModule<TRunner> AddWebApplication (
            string sourcePath,
            string virtualDirectoryName)
        {
            WebApplicationDeploymentModule<TRunner> deploymentModule = new WebApplicationDeploymentModule<TRunner>(
                sourcePath,
                virtualDirectoryName);
            AddModule(deploymentModule);
            return deploymentModule;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public DeploymentModulesRegistry<TRunner> ForEachModule(Action<DeploymentModule<TRunner>> action)
        {
            foreach (DeploymentModule<TRunner> module in modules)
                action(module);

            return this;
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private readonly DeploymentRunner<TRunner> runner;
        private readonly List<DeploymentModule<TRunner>> modules = new List<DeploymentModule<TRunner>>();
    }
}