namespace Flubu.Deployment
{
    public class MsmqDeploymentModule<TRunner> : DeploymentModule<TRunner>
        where TRunner : DeploymentRunner<TRunner>
    {
        public override DeploymentModule<TRunner> Deploy(DeploymentRunner<TRunner> runner)
        {
            throw new System.NotImplementedException();
        }
    }
}