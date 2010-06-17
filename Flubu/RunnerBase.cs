using System;
using Flubu.Builds;

namespace Flubu
{
    /// <summary>
    /// Task Runner helper.
    /// </summary>
    /// <typeparam name="TRunner">Concrete runner <see cref="BuildRunner{TRunner}"/></typeparam>
    public abstract class RunnerBase<TRunner> : BuildRunner<TRunner>
        where TRunner : BuildRunner<TRunner>
    {
        protected RunnerBase(string productId, string[] args) : base(productId)
        {
            Arguments = args;
        }

        /// <summary>
        /// Gets or sets Command line arguments pased to build runner.
        /// </summary>
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
            try
            {
                // actual run
                if (Arguments.Length == 0)
                {
                    RunTarget(DefaultTarget.TargetName);
                    Complete();
                    return 0;
                }

                foreach (string argument in Arguments)
                {
                    if (HasTarget(argument)) continue;

                    ScriptExecutionEnvironment.LogError(
                        "ERROR: The target '{0}' does not exist",
                        argument);
                    RunTarget("help");
                    return 2;
                }

                foreach (string argument in Arguments)
                {
                    RunTarget(argument);
                }

                Complete();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }
    }
}