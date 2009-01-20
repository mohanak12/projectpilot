using System;

namespace Headless
{
    public interface ITrafficCop
    {
        IStageRunnerFactory StageRunnerFactory { get; }

        TrafficCopControlSignal WaitForControlSignal(TimeSpan waitPeriod);
    }
}