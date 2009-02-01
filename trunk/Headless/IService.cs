using System.Diagnostics.CodeAnalysis;

namespace Headless
{
    public interface IService
    {
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ServiceInfo GetServiceInfo();
    }
}