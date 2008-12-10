using System;

namespace Accipio.Inservio.Framework
{
    public interface IInservioRemoteControl
    {
        object CallMethod(string domainNamespace, string serviceName, string methodName, params object[] args);

        void RegisterPlugIn(string plugInAssemblyPath, string domainNamespace);
    }
}