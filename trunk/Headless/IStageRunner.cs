using System;

namespace Headless
{
    public interface IStageRunner : IDisposable
    {
        void UpdateStatus(StageStatus status);
    }
}