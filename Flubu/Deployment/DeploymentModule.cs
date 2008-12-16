namespace Flubu.Deployment
{
    public abstract class DeploymentModule<TRunner>
        where TRunner : DeploymentRunner<TRunner>
    {
        public abstract DeploymentModule<TRunner> Deploy(DeploymentRunner<TRunner> runner);
    }
}