using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework
{
    public interface IProjectPilotConfiguration
    {
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        string ProjectPilotWebAppRootUrl { get; }
    }
}