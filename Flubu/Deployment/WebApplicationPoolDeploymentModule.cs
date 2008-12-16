namespace Flubu.Deployment
{
    public class WebApplicationPoolDeploymentModule<TRunner> : DeploymentModule<TRunner>
        where TRunner : DeploymentRunner<TRunner>
    {
        public override DeploymentModule<TRunner> Deploy(DeploymentRunner<TRunner> runner)
        {
            throw new System.NotImplementedException();
        }
    }
}