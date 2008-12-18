using System;
using System.Globalization;
using MbUnit.Framework;

namespace Accipio
{
    /// <summary>
    /// Selenium actions for ProjectPilot portal tests.
    /// </summary>
    public class ProjectPortalTestRunner : SeleniumTesterBase
    {
        /// <summary>
        /// Finds the button with specified Name.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase FindButton(string controlName)
        {
            FindInputControl(controlName);
            return this;
        }

        /// <summary>
        /// Finds the textbox with specified Name.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase FindTextBox(string controlName)
        {
            FindInputControl(controlName);
            return this;
        }

        /// <summary>
        /// Clicks on the specified button.
        /// </summary>
        /// <param name="controlName">The Name of the control which is contained in the input field.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public new SeleniumTesterBase ClickOnButton(string controlName)
        {
            string locator =
                String.Format(CultureInfo.InvariantCulture, "xpath=//input[contains(@name,'{0}')]", controlName);
            Selenium.Click(locator);
            Selenium.WaitForPageToLoad("10000");
            return this;
        }

        /// <summary>
        /// Types the specified text into textbox.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        /// <param name="searchText">The specified text.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase TypeText(string controlName, string searchText)
        {
            string locator =
                String.Format(CultureInfo.InvariantCulture, "xpath=//input[@name='{0}']", controlName);
            Selenium.Type(locator, searchText);
            Selenium.WaitForPageToLoad("10000");
            return this;
        }

        /// <summary>
        /// Confirm that orojects is listen on page.
        /// </summary>
        /// <param name="controlRef">The control ref.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase ProjectExists(string controlRef)
        {
            string locator =
                String.Format(CultureInfo.InvariantCulture, "xpath=//a[contains(@href,'{0}')]", controlRef);
            Assert.IsTrue(Selenium.IsElementPresent(locator), "Link with href='{0}' not found!", controlRef);
            return this;
        }

        /// <summary>
        /// Select the specified project.
        /// </summary>
        /// <param name="controlRef">The control ref.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase ProjectSelect(string controlRef)
        {
            string locator =
                String.Format(CultureInfo.InvariantCulture, "xpath=//a[@href='{0}']", controlRef);
            Selenium.Click(locator);
            Selenium.WaitForPageToLoad("10000");
            return this;
        }

        /// <summary>
        /// Finds the input control.
        /// </summary>
        /// <param name="controlName">Name of the control.</param>
        private void FindInputControl(string controlName)
        {
            string locator =
                String.Format(CultureInfo.InvariantCulture, "xpath=//input[@name='{0}']", controlName);
            Assert.IsTrue(Selenium.IsElementPresent(locator), "Control with name='{0}' not found!", controlName);
        }
    }
}