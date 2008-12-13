using System;
using System.Collections.Generic;

namespace Flubu
{
    public class FlubuRunnerTarget<TRunner>
        where TRunner : FlubuRunner<TRunner>
    {
        public FlubuRunnerTarget(
            TRunner runner, 
            string targetName)
        {
            this.runner = runner;
            this.targetName = targetName;
        }

        public ICollection<string> Dependencies
        {
            get { return dependencies; }
        }

        public string TargetName
        {
            get { return targetName; }
        }

        /// <summary>
        /// Specifies targets on which this target depends on.
        /// </summary>
        /// <param name="targetNames">The dependency target names.</param>
        /// <returns>This same instance of <see cref="FlubuRunnerTarget{TRunner}"/>.</returns>
        public FlubuRunnerTarget<TRunner> DependsOn (params string[] targetNames)
        {
            foreach (string targetName in targetNames)
                dependencies.Add(targetName);
            return this;
        }

        public void Execute()
        {
            runner.ScriptExecutionEnvironment.LogTargetStarted(this.targetName);

            runner.MarkTargetAsExecuted(this);
            runner.EnsureDependenciesExecuted(this.TargetName);

            // we can have actionless targets (that only depend on other targets)
            if (targetAction != null)
                targetAction(runner);

            runner.ScriptExecutionEnvironment.LogTargetFinished();
        }

        public FlubuRunnerTarget<TRunner> Do (Action<TRunner> targetAction)
        {
            if (this.targetAction != null)
                throw new ArgumentException("Target action was already set.");

            this.targetAction = targetAction;
            return this;
        }

        /// <summary>
        /// Sets the target as the default target for the runner.
        /// </summary>
        /// <returns>This same instance of <see cref="FlubuRunnerTarget{TRunner}"/>.</returns>
        public FlubuRunnerTarget<TRunner> SetAsDefault()
        {
            runner.SetDefaultTarget(this);
            return this;
        }

        private readonly List<string> dependencies = new List<string>();
        private readonly TRunner runner;
        private readonly string targetName;
        private Action<TRunner> targetAction;
    }
}