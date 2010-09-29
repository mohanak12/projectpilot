using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Flubu
{
    public interface IFlubuRunnerTarget
    {
        /// <summary>
        /// Gets the description of the target.
        /// </summary>
        /// <value>The description of the target.</value>
        string Description { get; }

        ICollection<string> Dependencies { get; }

        string TargetName { get; }

        Stopwatch TargetStopwatch { get; }

        /// <summary>
        /// Gets a value indicating whether this target is hidden. Hidden targets will not be
        /// visible in the list of targets displayed to the user as help.
        /// </summary>
        /// <value><c>true</c> if this target is hidden; otherwise, <c>false</c>.</value>
        bool IsHidden { get; }

        void Execute();
    }

    public class FlubuRunnerTarget<TRunner> : IFlubuRunnerTarget where TRunner : FlubuRunner<TRunner>
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

        /// <summary>
        /// Gets the description of the target.
        /// </summary>
        /// <value>The description of the target.</value>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Gets a value indicating whether this target is hidden. Hidden targets will not be
        /// visible in the list of targets displayed to the user as help.
        /// </summary>
        /// <value><c>true</c> if this target is hidden; otherwise, <c>false</c>.</value>
        public bool IsHidden
        {
            get { return isHidden; }
        }

        public string TargetName
        {
            get { return targetName; }
        }

        public Stopwatch TargetStopwatch
        {
            get { return targetStopwatch; }
        }

        /// <summary>
        /// Specifies targets on which this target depends on.
        /// </summary>
        /// <param name="targetNames">The dependency target names.</param>
        /// <returns>This same instance of <see cref="FlubuRunnerTarget{TRunner}"/>.</returns>
        public FlubuRunnerTarget<TRunner> DependsOn (params string[] targetNames)
        {
            foreach (string dependentTargetName in targetNames)
                dependencies.Add(dependentTargetName);
            return this;
        }

        public void Execute()
        {
            targetStopwatch.Start();
            runner.ScriptExecutionEnvironment.LogTargetStarted(this);

            runner.MarkTargetAsExecuted(this);
            runner.EnsureDependenciesExecuted(TargetName);

            // we can have actionless targets (that only depend on other targets)
            if (targetAction != null)
                targetAction(runner);

            TargetStopwatch.Stop();
            runner.ScriptExecutionEnvironment.LogTargetFinished(this);
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

        public FlubuRunnerTarget<TRunner> SetDescription(string description)
        {
            this.description = description;
            return this;
        }

        /// <summary>
        /// Sets the target as hidden. Hidden targets will not be
        /// visible in the list of targets displayed to the user as help.
        /// </summary>
        /// <returns>This same instance of <see cref="FlubuRunnerTarget{TRunner}"/>.</returns>
        public FlubuRunnerTarget<TRunner> SetAsHidden()
        {
            isHidden = true;
            return this;
        }

        private readonly List<string> dependencies = new List<string>();
        private string description;
        private bool isHidden;
        private readonly TRunner runner;
        private readonly string targetName;
        private Action<TRunner> targetAction;
        private readonly Stopwatch targetStopwatch = new Stopwatch();
    }
}