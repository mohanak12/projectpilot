using Accipio;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class ProjectPilotPortalTests
    {
        /// <summary>
        /// Test to check fluent interface with projectportal test runner.
        /// </summary>
        /// <remarks>It is used as example for csharp code generator.</remarks>
        [Test, Pending("Not ready yet")]
        public void GoToPortal()
        {
            using (ProjectPortalTestRunner runner = new ProjectPortalTestRunner())
            {
                runner
                    .SetDescription("descr")
                    .AddTag("tag1")
                    .ProjectPortal
                    .FindButton("Search")
                    .ClickOnButton("Search");
            }
        }
    }
}