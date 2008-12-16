namespace Flubu.Deployment
{
    public class ConcreteDeploymentRunner : DeploymentRunner<ConcreteDeploymentRunner>
    {
        public ConcreteDeploymentRunner(
            string scriptName, 
            string logFileName, 
            int howManyOldLogsToKeep) 
            : base(scriptName, logFileName, howManyOldLogsToKeep)
        {
        }
    }
}