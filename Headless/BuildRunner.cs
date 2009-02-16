using System;
using System.Collections.Generic;
using Headless.Configuration;
using log4net.Core;

namespace Headless
{
    public class BuildRunner : IBuildRunner
    {
        public BuildRunner(
            string projectId, 
            IProjectRegistry projectRegistry,
            IBuildTrafficSignals buildTrafficSignals, 
            IBuildStageRunnerFactory buildStageRunnerFactory,
            IHeadlessLogger logger)
        {
            this.projectId = projectId;
            this.projectRegistry = projectRegistry;
            this.buildTrafficSignals = buildTrafficSignals;
            this.buildStageRunnerFactory = buildStageRunnerFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public BuildReport Run()
        {
            BuildReport report = new BuildReport();
            Project projectToBuild = projectRegistry.GetProject(projectId);

            Log(Level.Info, "Starting the build runner");
            lock (this)
            {
                // construct a graph of build stages
                foreach (BuildStage stage in projectToBuild.BuildStages)
                    BuildDependencyGraph(stage);

                Log(Level.Debug, "Constructed build stages dependency graph");
            }

            while (true)
            {
                Log(Level.Debug, "New cycle started");

                lock (this)
                {
                    // retrieve the fresh info about stage statuses
                    foreach (BuildStage stage in stagesOrdered)
                    {
                        StageStatus status = GetStageStatus(stage);

                        if (status.Outcome == BuildOutcome.InProgress)
                            status.StageRunner.UpdateStatus(status);

                        Log(Level.Info, "Build stage '{0}' status: {1}", status.Stage.StageId, status.Outcome);
                    }
                }

                // check if the traffic buildTrafficSignals has signalled the runner to stop
                BuildTrafficCopSignal signal = buildTrafficSignals.WaitForControlSignal(TimeSpan.Zero);
                if (signal == BuildTrafficCopSignal.Stop)
                    break;

                bool stagesPending = false;

                lock (this)
                {
                    // check if any of the stages can be started
                    foreach (BuildStage stage in stagesOrdered)
                    {
                        StageStatus status = stagesStatuses[stage.StageId];
                        if (status.Outcome == BuildOutcome.Initial)
                        {
                            bool someDependenciesFailed = false;
                            bool allDependenciesSuccessful = true;

                            // check dependecy stages
                            foreach (BuildStage dependency in stage.DependsOn)
                            {
                                StageStatus dependencyStatus = GetStageStatus(dependency);

                                if (dependencyStatus.Outcome == BuildOutcome.Failed)
                                {
                                    // this stage will not be executed, so go to the next one
                                    someDependenciesFailed = true;
                                    allDependenciesSuccessful = false;
                                    break;
                                }
                                else if (dependencyStatus.Outcome == BuildOutcome.Successful)
                                    continue;

                                allDependenciesSuccessful = false;
                            }

                            if (someDependenciesFailed)
                                status.MarkAsNotExecuted();
                            else if (allDependenciesSuccessful)
                            {
                                // we can start this stage
                                Log(Level.Info, "Starting stage '{0}'", status.Stage.StageId);

                                IStageRunner stageRunner = this.buildStageRunnerFactory.CreateStageRunner(status.Stage);
                                status.PrepareToStart(stageRunner);
                                stageRunner.StartStage();
                                // update the status immediatelly
                                stageRunner.UpdateStatus(status);

                                stagesPending = true;
                            }
                            else
                                stagesPending = true;
                        }
                    }
                }

                if (stagesPending)
                    Log(Level.Debug, "Some build stages are still pending");
                else
                {
                    Log(Level.Debug, "The build has been executed");
                    break;
                }

                Log(Level.Debug, "Waiting for the traffic cop");

                // check if the traffic buildTrafficSignals has signalled the runner to stop
                signal = buildTrafficSignals.WaitForControlSignal(pollPeriod);
                if (signal == BuildTrafficCopSignal.Stop)
                    break;
            }

            Log(Level.Info, "Stopping the build runner");

            foreach (BuildStage stage in stagesOrdered)
            {
                BuildStageReport stageReport = new BuildStageReport();
                stageReport.StageOutcome = GetStageStatus(stage).Outcome;
                report.StageReports.Add(stage.StageId, stageReport);
            }

            return report;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <code>false</code>, cleans up native resources. 
        /// If <code>true</code> cleans up both managed and native resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (false == disposed)
            {
                // TODO: clean native resources         

                if (disposing)
                {
                    foreach (StageStatus stageStatus in stagesStatuses.Values)
                        stageStatus.Dispose();
                }

                disposed = true;
            }
        }

        protected void Log (Level level, string format, params object[] args)
        {
            logger.Log(
                LogEvent.ForProject(
                    projectId, 
                    level, 
                    format, 
                    args));
        }

        private void BuildDependencyGraph(BuildStage stage)
        {
            foreach (BuildStage dependency in stage.DependsOn)
                BuildDependencyGraph(dependency);

            if (false == stagesOrdered.Contains(stage))
            {
                stagesOrdered.Add(stage);
                stagesStatuses.Add(stage.StageId, new StageStatus(stage));
            }
        }

        private StageStatus GetStageStatus(BuildStage stage)
        {
            return stagesStatuses[stage.StageId];
        }

        private bool disposed;
        private readonly IHeadlessLogger logger;
        private List<BuildStage> stagesOrdered = new List<BuildStage>();
        private IBuildStageRunnerFactory buildStageRunnerFactory;
        private Dictionary<string, StageStatus> stagesStatuses = new Dictionary<string, StageStatus>();
        private TimeSpan pollPeriod = TimeSpan.FromSeconds(10);
        private readonly string projectId;
        private readonly IProjectRegistry projectRegistry;
        private IBuildTrafficSignals buildTrafficSignals;
    }
}
