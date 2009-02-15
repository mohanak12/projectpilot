using System;
using System.Threading;

namespace Headless.Threading
{
    public interface IThreadFactory : IDisposable
    {
        Thread CreateThread(string threadName, ThreadStart threadStart);

        void WaitForAllThreadsToStop(TimeSpan timeout);
    }
}