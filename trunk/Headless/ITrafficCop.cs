using System;

namespace Headless
{
    public interface ITrafficCop
    {
        TrafficCopControlSignal WaitForControlSignal(TimeSpan waitPeriod);
    }
}