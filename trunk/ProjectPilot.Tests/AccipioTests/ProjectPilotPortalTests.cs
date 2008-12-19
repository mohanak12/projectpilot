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
        [Test, Pending("Used as example")]
        public void ClickSearchButton()
        {
            using (ProjectPortalTestRunner runner = new ProjectPortalTestRunner())
            {
                runner
                    .SetDescription("descr")
                    .AddTag("tag1");

                runner
                    //description of action GoToPortal
                    .GoToPortal("/MobitelLdapGui")
                    //description of action FindButton
                    .FindButton("Search")
                    //description of action ClickOnButton
                    .ClickOnButton("Search");
            }
        }
    }
}