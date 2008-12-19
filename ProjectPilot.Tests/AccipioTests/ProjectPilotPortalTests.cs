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
        [Test, Explicit("Used as example how generated code should look like")]
        public void ClickSearchButton()
        {
            using (ProjectPortalTestRunner runner = new ProjectPortalTestRunner())
            {
                runner
                    .SetDescription("descr")
                    .AddTag("tag1");

                runner
                    //description of action GoToPortal
                    .GoToPortal("/projectpilot")
                    //description of action FindButton
                    .FindButton("Search")
                    //description of action ClickOnButton
                    .ClickOnButton("Search")
                    //find search text box
                    .FindTextBox("SearchQuery")
                    //type text
                    .TypeText("SearchQuery", "some text...");
            }
        }
    }
}