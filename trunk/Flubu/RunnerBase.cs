using System;
using Flubu.Builds;

namespace Flubu
{
    public abstract class RunnerBase<TRunner> : BuildRunner<TRunner>
        where TRunner : BuildRunner<TRunner>
    {
        protected RunnerBase(string productId, string[] args) : base(productId)
        {
            Arguments = args;
        }

        public string[] Arguments { get; set; }

        public abstract void Configure();

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