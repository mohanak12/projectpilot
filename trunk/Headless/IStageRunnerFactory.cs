using Headless.Configuration;

namespace Headless
{
    public interface IStageRunnerFactory
    {
        IStageRunner CreateStageRunner(BuildStage stage);
    }
}