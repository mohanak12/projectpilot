using Accipio;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Contains test cases which cover search and selection of portal projects.
    /// </summary>
    [TestFixture]
    public class ProjectPortalHomeTestSuite
    {
        /// <summary>
        /// Opens Project portal home page in browser.
        /// </summary>
        [Test, Pending("Used as example")]
        [Category("")]
        public void GoToPortal()
        {
            using (ProjectPortalTestRunner runner = new ProjectPortalTestRunner())
            {
                runner
                    .SetDescription("Opens Project portal home page in browser.")
                    .AddTag("ProjectPortal.Home");

                runner
                    // Open the ProjectPilot web site 'http://localhost/ProjectPortal' in the browser.
                    .GoToPortal("http://localhost/ProjectPortal")
                    // Find button with id 'Search'.
                    .FindButton("Search")
                    // Find textbox with id 'SearchQuery'.
                    .FindTextBox("SearchQuery");
            }
        }

        /// <summary>
        /// Finds and select a project on Project portal.
        /// </summary>
        [Test, Pending("Used as example")]
        [Category("")]
        public void SelectProjectEbsy()
        {
            using (ProjectPortalTestRunner runner = new ProjectPortalTestRunner())
            {
                runner
                    .SetDescription("Finds and select a project on Project portal.")
                    .AddTag("ProjectPortal.Home");

                runner
                    // Open the ProjectPilot web site 'http://localhost/ProjectPortal' in the browser.
                    .GoToPortal("http://localhost/ProjectPortal")
                    // Type 'Ebsy' into textbox with id 'SearchQuery'.
                    .TypeText("SearchQuery", "Ebsy")
                    // Confirm that project '/ProjectPilot.Portal/ProjectView/Overview/ebsy/' is listed on page.
                    .ProjectExists("/ProjectPilot.Portal/ProjectView/Overview/ebsy/")
                    // Select project '/ProjectPilot.Portal/ProjectView/Overview/ebsy/'.
                    .ProjectSelect("/ProjectPilot.Portal/ProjectView/Overview/ebsy/");
            }
        }
    }
}