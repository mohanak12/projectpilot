using System;

namespace Accipio
{
    public interface ICodeWriter : IDisposable
    {
        void Close();

        void WriteLine(string line);
    }
}