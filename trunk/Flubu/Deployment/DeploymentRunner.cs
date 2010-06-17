namespace Flubu.Deployment
{
    /// <summary>
    /// A specialization of the <see cref="FlubuRunner{TRunner}"/> oriented towards deploying 
    /// .NET software.
    /// </summary>
    /// <typeparam name="TRunner">The concrete type of the runner.</typeparam>
    public class DeploymentRunner<TRunner> : FlubuRunner<TRunner>
        where TRunner : DeploymentRunner<TRunner>
    {
        public DeploymentRunner(
            string scriptName, 
            string logFileName, 
            int howManyOldLogsToKeep) 
            : base(scriptName, logFileName, howManyOldLogsToKeep)
        {
            deploymentModules = new DeploymentModulesRegistry<TRunner>(this);
        }

        public DeploymentModulesRegistry<TRunner> DeploymentModules
        {
            get { return deploymentModules; }
        }

        public string DestinationPathRoot
        {
            get { return destinationPathRoot; }
        }

        public string SourcePathRoot
        {
            get { return sourcePathRoot; }
        }

        public virtual TRunner Deploy()
        {
            deploymentModules.ForEachModule(
                delegate(DeploymentModule<TRunner> module)
                    {
                        module.Deploy(this);
                    });

            return ReturnThisTRunner();
        }

        public TRunner SetDestinationPathRoot(string destinationPathRoot)
        {
            this.destinationPathRoot = destinationPathRoot;
            return ReturnThisTRunner();
        }

        public TRunner SetSourcePathRoot(string sourcePathRoot)
        {
            this.sourcePathRoot = sourcePathRoot;
            return ReturnThisTRunner();
        }

        private DeploymentModulesRegistry<TRunner> deploymentModules;
        private string destinationPathRoot;
        private string sourcePathRoot = ".";
    }
}
