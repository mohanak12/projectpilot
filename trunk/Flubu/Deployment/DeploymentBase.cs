using System.Diagnostics.CodeAnalysis;

namespace Flubu.Deployment
{
    public abstract class DeploymentBase<TRunner> : DeploymentRunner<TRunner>
        where TRunner : DeploymentRunner<TRunner>
    {
        protected DeploymentBase(string scriptName, string[] args) 
            : base(scriptName, "deploy.log", 5)
        {
            Arguments = args;
        }

        /// <summary>
        /// Gets or sets Command line arguments pased to build runner.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] Arguments { get; set; }

        /// <summary>
        /// Method that will configure available build targets.
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// Target runner.
        /// </summary>
        /// <returns>0 if all ok.</returns>
        public virtual int Run()
        {
            return FlubuRunner<TRunner>.Run(this, Arguments);
        }
    }
}
