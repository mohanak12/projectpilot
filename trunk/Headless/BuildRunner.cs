using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Headless.Configuration;

namespace Headless
{
    public class BuildRunner : IDisposable
    {
        public BuildRunner(ITrafficCop cop, Project projectToBuild)
        {
            this.cop = cop;
            this.projectToBuild = projectToBuild;
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

        public void Run()
        {
            lock (this)
            {
                // construct a graph of build stages
                foreach (BuildStage stage in projectToBuild.BuildStages)
                    BuildDependencyGraph(stage);
            }

            while (true)
            {
                lock (this)
                {
                    // retrieve the fresh info about stage statuses
                    foreach (BuildStage stage in stagesOrdered)
                    {
                        StageStatus status = GetStageStatus(stage);

                        if (status.Outcome == StageOutcome.InProgress)
                            status.StageRunner.UpdateStatus(status);
                    }
                }

                // check if the traffic cop has signalled the runner to stop
                TrafficCopControlSignal signal = cop.WaitForControlSignal(TimeSpan.Zero);
                if (signal == TrafficCopControlSignal.Stop)
                    break;

                bool stagesInWaiting = false;

                lock (this)
                {
                    // check if any of the stages can be started
                    foreach (BuildStage stage in stagesOrdered)
                    {
                        StageStatus status = stagesStatuses[stage.StageId];
                        if (status.Outcome == StageOutcome.Initial)
                        {
                            bool someDependenciesFailed = false;
                            bool allDependenciesSuccessful = true;

                            // check dependecy stages
                            foreach (BuildStage dependency in stage.DependsOn)
                            {
                                StageStatus dependencyStatus = GetStageStatus(stage);

                                if (dependencyStatus.Outcome == StageOutcome.Failed)
                                {
                                    // this stage will not be executed, so go to the next one
                                    someDependenciesFailed = true;
                                    allDependenciesSuccessful = false;
                                    break;
                                }
                                else if (dependencyStatus.Outcome == StageOutcome.Successful)
                                    continue;

                                allDependenciesSuccessful = false;
                            }

                            if (someDependenciesFailed)
                                status.MarkAsNotExecuted();
                            else if (allDependenciesSuccessful)
                            {
                                // we can start this stage
                                IStageRunner stageRunner = this.cop.StageRunnerFactory.CreateStageRunner(status.Stage);
                                status.PrepareToStart(stageRunner);
                            }
                            else
                                stagesInWaiting = true;
                        }
                    }
                }

                // check if the traffic cop has signalled the runner to stop
                signal = cop.WaitForControlSignal(pollPeriod);
                if (signal == TrafficCopControlSignal.Stop)
                    break;
            }
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
                    // TODO: clean managed resources            
                }

                disposed = true;
            }
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

        private ITrafficCop cop;
        private bool disposed;
        private Project projectToBuild;
        private List<BuildStage> stagesOrdered = new List<BuildStage>();
        private Dictionary<string, StageStatus> stagesStatuses = new Dictionary<string, StageStatus>();
        private TimeSpan pollPeriod = TimeSpan.FromSeconds(10);
    }
}
