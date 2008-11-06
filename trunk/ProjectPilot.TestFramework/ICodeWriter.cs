using System;

namespace ProjectPilot.TestFramework
{
    public interface ICodeWriter : IDisposable
    {
        void Close();
        void WriteLine(string line);
    }
}