using System;
using System.IO;
using Flubu.Tasks.Iis;
using Flubu.Tasks.Iis.Iis6;

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
            destinationPath = sourcePath;
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
            Action<Iis6CreateWebApplicationTask> callback)
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
            Iis6CreateWebApplicationTask task = new Iis6CreateWebApplicationTask();
            task.ApplicationName = virtualDirectoryName;
            task.LocalPath = destinationFullPath;
            task.Mode = CreateWebApplicationMode.UpdateIfExists;

            if (customizeWebApplicationCallback != null)
                customizeWebApplicationCallback(task);

            runner.RunTask(task);
            return this;
        }

        private Action<Iis6CreateWebApplicationTask> customizeWebApplicationCallback;
        private readonly string destinationPath;
        private readonly string sourcePath;
        private readonly string virtualDirectoryName;
    }
}