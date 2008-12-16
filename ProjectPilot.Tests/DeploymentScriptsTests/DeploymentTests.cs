using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flubu.Deployment;
using Flubu.Tasks.Iis;
using MbUnit.Framework;

namespace ProjectPilot.Tests.DeploymentScriptsTests
{
    [TestFixture]
    public class DeploymentTests
    {
        [Test, Pending("Igor: Stuff to finish")]
        public void Test()
        {
            using (ConcreteDeploymentRunner runner = new ConcreteDeploymentRunner("test", "test.log", 1))
            {
                // define the deployment modules
                runner
                    .SetSourcePathRoot(@".")
                    .SetDestinationPathRoot(@"d:\brisi\pp")

                    .DeploymentModules
                    .AddWebApplication("ProjectPilot.Portal", "ProjectPilot")
                    .CustomizeWebApplication(
                    delegate(CreateVirtualDirectoryTask task)
                        {
                        })
                    ;

                runner
                    .Deploy();
            }
        }
    }
}
