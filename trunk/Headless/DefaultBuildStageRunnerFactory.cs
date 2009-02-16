using Headless.Configuration;

namespace Headless
{
    public class DefaultBuildStageRunnerFactory : IBuildStageRunnerFactory
    {
        public DefaultBuildStageRunnerFactory(IHeadlessLogger headlessLogger)
        {
            this.headlessLogger = headlessLogger;
        }

        public IStageRunner CreateStageRunner(BuildStage stage)
        {
            return new LocalStageRunner(headlessLogger);
        }

        private readonly IHeadlessLogger headlessLogger;
    }
}