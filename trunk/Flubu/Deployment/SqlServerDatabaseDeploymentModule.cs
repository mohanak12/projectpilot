namespace Flubu.Deployment
{
    public class SqlServerDatabaseDeploymentModule<TRunner> : DeploymentModule<TRunner>
        where TRunner : DeploymentRunner<TRunner>
    {
        public override DeploymentModule<TRunner> Deploy(DeploymentRunner<TRunner> runner)
        {
            throw new System.NotImplementedException();
        }
    }
}