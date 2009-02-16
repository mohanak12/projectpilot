using System;

namespace Headless
{
    public interface IBuildRunner : IDisposable
    {
        BuildReport Run();
    }
}