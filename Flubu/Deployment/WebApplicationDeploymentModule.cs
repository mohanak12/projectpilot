using System;
using System.IO;
using Flubu.Tasks.Iis;

namespace Flubu.Deployment
{
    public class WebApplicationDeploymentModule<TRunner> : DeploymentModule<TRunner>
        where TRunner : DeploymentRunner<TRunner> 
    {
        public WebApplicationDeploymentModule(
            string sourcePath,
            string virtualDirectoryName)
        {
            this.sourcePath = sourcePath;
            this.destinationPath = sourcePath;
            this.virtualDirectoryName = virtualDirectoryName;
        }

        public WebApplicationDeploymentModule(
            string sourcePath,
            string destinationPath,
            string virtualDirectoryName)
        {
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
            this.virtualDirectoryName = virtualDirectoryName;
        }

        public WebApplicationDeploymentModule<TRunner> CustomizeWebApplication(
            Action<CreateVirtualDirectoryTask> callback)
        {
            customizeWebApplicationCallback = callback;
            return this;
        }

        public override DeploymentModule<TRunner> Deploy(DeploymentRunner<TRunner> runner)
        {
            // copy files
            string sourceFullPath = Path.Combine(runner.SourcePathRoot, sourcePath);
            string destinationFullPath = Path.Combine(runner.DestinationPathRoot, destinationPath);

            runner
                .CopyDirectoryStructure(sourceFullPath, destinationFullPath, true);

            // create virtual directory
            CreateVirtualDirectoryTask task = new CreateVirtualDirectoryTask(
                virtualDirectoryName,
                destinationFullPath,
                CreateVirtualDirectoryMode.UpdateIfExists);

            if (customizeWebApplicationCallback != null)
                customizeWebApplicationCallback(task);

            runner.RunTask(task);
            return this;
        }

        private Action<CreateVirtualDirectoryTask> customizeWebApplicationCallback;
        private readonly string destinationPath;
        private readonly string sourcePath;
        private readonly string virtualDirectoryName;
    }
}