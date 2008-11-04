using System.Diagnostics.CodeAnalysis;

namespace ProjectPilot.Framework.Runners
{
    public class DefaultRunner : IRunner
    {
        public DefaultRunner (IProjectRegistry projectRegistry)
        {
            this.projectRegistry = projectRegistry;
        }

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private readonly IProjectRegistry projectRegistry;
    }
}