using Headless.Configuration;

namespace Headless
{
    public interface IBuildStageRunnerFactory
    {
        IStageRunner CreateStageRunner(BuildStage stage);
    }
}