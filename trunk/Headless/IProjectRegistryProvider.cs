using System.Diagnostics.CodeAnalysis;
using Headless.Configuration;

namespace Headless
{
    public interface IProjectRegistryProvider
    {
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ProjectRegistry GetProjectRegistry();
    }
}