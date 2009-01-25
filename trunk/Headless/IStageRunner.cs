using System;
using Headless.Configuration;

namespace Headless
{
    public interface IStageRunner : IDisposable
    {
        void SetBuildStage(BuildStage buildStage);

        void StartStage();
        
        void UpdateStatus(StageStatus status);
    }
}