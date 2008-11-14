using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using MbUnit.Framework;
using Selenium;

namespace ProjectPilot.TestFramework
{
    /// <summary>
    /// Tester class for testing Web application using Selenium. The class provides a fluent interface 
    /// for testing.
    /// </summary>
    public class SeleniumTesterBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumTesterBase"/> class.
        /// </summary>
        public SeleniumTesterBase()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            browserType = (BrowserType) Enum.Parse(typeof (BrowserType),
                                                   ConfigurationManager.AppSettings["BrowserType"],
                                                   true);
            testMachine = ConfigurationManager.AppSettings["TestMachine"];
            seleniumPort = int.Parse(ConfigurationManager.AppSettings["SeleniumPort"],
                                     CultureInfo.InvariantCulture);
            seleniumSpeed = ConfigurationManager.AppSettings["SeleniumSpeed"];
            browserUrl = ConfigurationManager.AppSettings["BrowserUrl"];
            targetUrl = new Uri(ConfigurationManager.AppSettings["TargetUrl"]);

            string browserExe;

            switch (browserType)
            {
                case BrowserType.InternetExplorer:
                    browserExe = "*iexplore";
                    break;
                case BrowserType.Firefox:
                    browserExe = "*firefox";
                    break;

                default:
                    throw new NotSupportedException();
            }

            selenium = new DefaultSelenium(testMachine, seleniumPort, browserExe, browserUrl);
            selenium.Start();

            Console.WriteLine("Started Selenium session (browser type={0})",
                              browserType);

            // sets the speed of execution of GUI commands
            if (false == String.IsNullOrEmpty(seleniumSpeed))
                selenium.SetSpeed(seleniumSpeed);
        }

        /// <summary>
        /// Gets the underlying Selenium object.
        /// </summary>
        /// <value>The underlying Selenium object.</value>
        [CLSCompliant(false)]
        public ISelenium Selenium
        {
            get { return selenium; }
        }

        /// <summary>
        /// Check the Ansfer confirmation
        /// </summary>
        /// <param name="confirmationText">Confirmation text</param>
        /// <param name="confirm">is it confirmed ?</param>
        /// <returns>The same <see cref="SeleniumTesterBase"/> object.</returns>
        public SeleniumTesterBase AnswerToConfirmation(string confirmationText, bool confirm)
        {
            Assert.IsTrue(selenium.IsConfirmationPresent());
            Assert.AreEqual(confirmationText, selenium.GetConfirmation());

            if (confirm)
                selenium.ChooseOkOnNextConfirmation();
            else
                selenium.ChooseCancelOnNextConfirmation();

            return this;
        }

        /// <summary>
        /// Asserts that the button with controlId is available on the page.
        /// </summary>
        /// <param name="controlId">Id of the button.</param>
        /// <returns>The same <see cref="SeleniumTesterBase"/> object.</returns>
        public SeleniumTesterBase AssertButtonAvailable(string controlId)
        {
            Assert.IsFalse(selenium.IsElementPresent(
                               String.Format(CultureInfo.InvariantCulture,
                                             "xpath=//a[contains(@href,'{0}')]", controlId)));

            return this;
        }

        /// <summary>
        /// Asserts that the DropDown contains given values.
        /// </summary>
        /// <param name="controlId">Id of dropDown control to assert.</param>
        /// <param name="parameters">Selection values that dropDown has to contain.</param>
        /// <returns>The same <see cref="SeleniumTesterBase"/> object.</returns>
        public SeleniumTesterBase AssertDropDownContainsValues(string controlId, params string[] parameters)
        {
            string locatorForDropDown = string.Format(CultureInfo.CurrentCulture,
                                                      "xpath=//select[contains(@id,'{0}')]", controlId);
            string[] itemValues = selenium.GetSelectOptions(locatorForDropDown);
            List<string> list = new List<string>(itemValues);
            Assert.AreEqual(parameters.Length, itemValues.Length);
            foreach (string parameter in parameters)
            {
                Assert.IsTrue(list.Contains(parameter));
            }
            return this;
        }

        /// <summary>
        /// Asserts the page displayed a specified list of validation error codes. If at least one error is missing, 
        /// the method fails.
        /// </summary>
        /// <param name="errorCodes">The validation error codes which have to be present on the page.</param>
        /// <returns>The same <see cref="SeleniumTesterBase"/> object.</returns>
        public SeleniumTesterBase AssertHasErrors(params string[] errorCodes)
        {
            List<string> errorCodesList = new List<string>(errorCodes);

            foreach (string errorCode in errorCodesList)
            {
                Assert.IsTrue(selenium.IsTextPresent(errorCode),
                    "Error '{0}' is missing.", errorCode);
            }

            return this;
        }

        /// <summary>
        /// Asserts the specified HTML element has the expected text.
        /// </summary>
        /// <param name="locator">The locator for the HTML element.</param>
        /// <param name="expectedText">The expected text.</param>
        /// <returns>The same <see cref="SeleniumTesterBase"/> object.</returns>
        public SeleniumTesterBase AssertHtmlText(string locator, string expectedText)
        {
            string actualText = Selenium.GetText(locator);
            Assert.AreEqual(expectedText, actualText, "Unexpected HTML text");

            return this;
        }

        /// <summary>
        /// Asserts that the application is currently on the specified page.
        /// </summary>
        /// <param name="pageName">The name of the page.</param>
        /// <param name="pageId">The ID of the dialog.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase AssertIsOnPage(string pageName, string pageId)
        {
            Uri absoluteUrl = new Uri(selenium.GetLocation());
            string localPath = absoluteUrl.LocalPath;
            Assert.AreEqual(pageName, Path.GetFileName(localPath),
                            String.Format(CultureInfo.InvariantCulture,
                                          "Page '{0}' was expected, actually it is '{1}'.",
                                          pageName,
                                          Path.GetFileName(localPath)));

            if (pageId != null)
            {
                AssertLabelText("DialogIdLabel", pageId);
            }

            return this;
        }

        /// <summary>
        /// Asserts that the Label with specific Id has a specific Text set
        /// </summary>
        /// <param name="controlId">ControlId (or part of it) of the label we are chcking</param>
        /// <param name="labelText">Text to which we are checking against</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase AssertLabelText(string controlId, string labelText)
        {
            string text = selenium.GetText("xpath=//span[contains(@id,'" + controlId + "')]");
            Assert.AreEqual(labelText,
                            text,
                            "Label Text '{0}' was expected, actually it is '{1}'",
                            labelText,
                            text);

            return this;
        }

        /// <summary>
        /// Asserts that the TextBox with specific Id has a specific Value set
        /// </summary>
        /// <param name="controlId">ControlId (or part of it) of the textbox we are chcking</param>
        /// <param name="textBoxValue">Value to which we are checking against</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase AssertTextBoxValue(string controlId, string textBoxValue)
        {
            string text = selenium.GetValue("xpath=//input[contains(@id,'" + controlId + "')]");
            Assert.AreEqual(textBoxValue,
                            text,
                            "TextBox Text '{0}' was expected, actually it is '{1}",
                            textBoxValue,
                            text);

            return this;
        }

        /// <summary>
        /// Asserts that the value is part of selection.
        /// </summary>
        /// <param name="tariffDisplayInfo">value to select, which is checked for its appearance.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase AssertIsLabelTextAvailable(string tariffDisplayInfo)
        {
            Assert.IsTrue(selenium.IsElementPresent("xpath=//label[text()='" + tariffDisplayInfo + "']"),
                          "Tariff '{0}' is not available", tariffDisplayInfo);
            return this;
        }

        /// <summary>
        /// Asserts that the value is not part of selection.
        /// </summary>
        /// <param name="controlValue">value to select, which is checked for its appearance.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase AssertIsOptionUnavailable(string controlValue)
        {
            Assert.IsFalse(selenium.IsElementPresent("xpath=//input[contains(@value,'" + controlValue + "')]"));
            return this;
        }

        /// <summary>
        /// Asserts that the Label doesn't exist
        /// </summary>
        /// <param name="controlId">ControlId (or part of it) of the label we are checking if it exists</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase AssertNoLabel(string controlId)
        {
            string text = null;
            try
            {
                text = selenium.GetText("xpath=//span[contains(@id,'" + controlId + "')]");
            }
            catch (SeleniumException)
            {
            }

            Assert.AreEqual(null,
                            text,
                            "Label with Id '{0}' was not expected but was found.",
                            controlId);
            return this;
        }

        /// <summary>
        /// Asserts that the RadioButton contains given values.
        /// </summary>
        /// <param name="groupName">Name of group of radio buttons.</param>
        /// <param name="parameters">Raddio button values that have to be in given group.</param>
        /// <returns>The same <see cref="SeleniumTesterBase"/> object.</returns>
        public SeleniumTesterBase AssertRadioButtonContainsValues(string groupName, params string[] parameters)
        {
            Assert.AreEqual(parameters.Length,
                            selenium.GetXpathCount(String.Format(CultureInfo.InvariantCulture,
                                                                 "//input[@type='radio' and @name='{0}']", groupName)),
                            "Radio button '{0}' does not contain the expected number of values.", groupName);
            foreach (string parameter in parameters)
            {
                if (false == String.IsNullOrEmpty(parameter))
                    Assert.IsTrue(
                        selenium.IsElementPresent(String.Format(CultureInfo.InvariantCulture,
                                                                "xpath=//input[@type='radio' and @name='{0}' and @value='{1}']",
                                                                groupName, parameter)),
                        "Radio button '{0}' does not contain the expected value '{1}'.", groupName, parameter);
            }
            return this;
        }

        /// <summary>
        /// Asserts that the RadioButton contains disabled value.
        /// </summary>
        /// <param name="groupName">Name of group of radio buttons.</param>
        /// <param name="value">Value of the selected radio button.</param>
        /// <returns>The same <see cref="SeleniumTesterBase"/> object.</returns>
        public SeleniumTesterBase AssertIsRadioButtonValueDisabled(string groupName, string value)
        {
            try
            {
                Assert.IsFalse(
                    selenium.IsEditable(String.Format(CultureInfo.InvariantCulture,
                                                      "xpath=//input[@type='radio' and @name='{0}' and @value='{1}']",
                                                      groupName, value)),
                    "Radio button '{0}' value '{1}' is not disabled.", groupName, value);
            }
            catch (SeleniumException)
            {
                Assert.Fail("Radio button '{0}' dose not contain value '{1}'.", groupName, value);
            }

            return this;
        }


        /// <summary>
        /// Asserts that specified value in radio is selected.
        /// </summary>
        /// <param name="controlValue">the Value property of the radio button</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase AssertIsRadioButtonValueSelected(string controlValue)
        {
            Assert.IsTrue(selenium.IsChecked(String.Format(CultureInfo.InvariantCulture,
                                                           "xpath=//input[@type='radio' and @value='{0}']", controlValue)));
            return this;
        }

        /// <summary>
        /// Assert checkbox state
        /// </summary>
        /// <param name="selected">true if checkbox is seleced, otherwise false</param>
        /// <param name="locator">checkbox control locator (e.g."xpath=//input[contains(@id,'ctl00_MainContentPlaceholder_ContentPanel1_CheckBox1')]")</param>
        /// <returns></returns>
        public SeleniumTesterBase AssertCheckBoxState(bool selected, string locator)
        {
            Assert.AreEqual(selected,
                            selenium.IsChecked(locator));
            return this;
        }

        /// <summary>
        /// Asserts that the drop down control has the specified value selected.
        /// </summary>
        /// <param name="dropDownControlId">The ID of the drop down control.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase AssertSelectedDropDownValue(string dropDownControlId, object expectedValue)
        {
            string locatorForDropDown = string.Format(CultureInfo.CurrentCulture,
                                                      "xpath=//select[contains(@id,'{0}')]", dropDownControlId);
            string selectedValue = selenium.GetSelectedValue(locatorForDropDown);

            Assert.AreEqual(expectedValue.ToString(), selectedValue);

            return this;
        }

        /// <summary>
        /// Presses the "Back" button on the browser.
        /// </summary>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase BrowserBackButton()
        {
            selenium.GoBack();
            return Pause();
        }

        /// <summary>
        /// Clears the value of the specified input control.
        /// </summary>
        /// <param name="inputControlId">The ID of the input control.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase ClearValue(string inputControlId)
        {
            return EnterValue(inputControlId, String.Empty);
        }

        /// <summary>
        /// Clicks the on the specified link.
        /// </summary>
        /// <param name="controlId">The ID of the control which is contained in the link.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase ClickOnLink(string controlId)
        {
            selenium.Click(String.Format(CultureInfo.InvariantCulture,
                                         "xpath=//a[contains(@href,'{0}')]", controlId));
            selenium.WaitForPageToLoad("10000");
            //Pause();
            return this;
        }

        /// <summary>
        /// Clicks the on the specified button.
        /// </summary>
        /// <param name="controlId">The ID of the control which is contained in the input field.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase ClickOnButton(string controlId)
        {
            selenium.Click(String.Format(CultureInfo.InvariantCulture,
                                         "xpath=//input[contains(@id,'{0}')]", controlId));
            selenium.WaitForPageToLoad("10000");
            //Pause();
            return this;
        }

        /// <summary>
        /// Clicks the on the specified link button.
        /// </summary>
        /// <param name="controlId">The ID of the control which is contained in the input field.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase ClickOnLinkButton(string controlId)
        {
            selenium.Click(String.Format(CultureInfo.InvariantCulture,
                                         "xpath=//a[contains(@id,'{0}')]", controlId));
            selenium.WaitForPageToLoad("10000");
            //Pause();
            return this;
        }

        /// <summary>
        /// Clicks the on the specified link and then expects a confirmation message box to appear.
        /// If confirmed then it goes to load a new page.
        /// </summary>
        /// <param name="controlId">The ID of the control which is contained in the link.</param>
        /// <param name="confirmationText">The expected confirmation text in the message box.</param>
        /// <param name="confirm">if set to <c>true</c>, the action will be confirmed. Otherwise it will be cancelled.</param>
        /// <param name="linkButton">if set to true the button is a LinkButton</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase ClickOnWithConfirmation(
            string controlId,
            string confirmationText,
            bool confirm,
            bool linkButton)
        {
            string controlName = "input";
            if (linkButton)
            {
                controlName = "a";
            }
            // first set up the non-default Selenium behaviour
            if (false == confirm)
                selenium.ChooseCancelOnNextConfirmation();

            selenium.Click(String.Format(CultureInfo.InvariantCulture,
                                         "xpath=//{0}[contains(@id,'{1}')]", controlName, controlId));

            Assert.IsTrue(selenium.IsConfirmationPresent());
            Assert.AreEqual(confirmationText, selenium.GetConfirmation());

            // only if confirmed will a new page be actually loaded
            if (confirm)
                selenium.WaitForPageToLoad("10000");

            return this;
        }

        /// <summary>
        /// Sets a Radio Button (which is passed in as the value property of the control)
        /// </summary>
        /// <param name="controlValue">the Value property of the radio button</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase SetRadioButtonByValue(string controlValue)
        {
            selenium.Check("xpath=//input[contains(@value,'" + controlValue + "')]");
            return this;
        }

        /// <summary>
        /// Sets a Radio Button containing string in id
        /// </summary>
        /// <param name="controlId">the Id property of the radio button</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase SetRadioButtonById(string controlId)
        {
            selenium.Check("xpath=//input[contains(@id,'" + controlId + "')]");
            return this;
        }

        /// <summary>
        /// Checks or unchecks a specific CheckBox
        /// </summary>
        /// <param name="controlId">Control ID of the control (can be only a part of it)</param>
        /// <param name="value">true = check it, false = uncheck it</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase SetCheckBox(string controlId, bool value)
        {
            if (value)
            {
                selenium.Check(String.Format(CultureInfo.InvariantCulture,
                                             "xpath=//input[contains(@id,'{0}')]", controlId));
            }
            else
            {
                selenium.Uncheck(String.Format(CultureInfo.InvariantCulture,
                                               "xpath=//input[contains(@id,'{0}')]", controlId));
            }
            return this;
        }

        /// <summary>
        /// Enters the text into the input field.
        /// </summary>
        /// <param name="inputControlId">The ID of the input control.</param>
        /// <param name="value">The text value to be entered.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase EnterValue(string inputControlId, string value)
        {
            selenium.Type(String.Format(CultureInfo.InvariantCulture,
                                        "xpath=//input[contains(@id,'{0}') or contains(@name,'{0}')]", inputControlId),
                          value);
            return this;
        }

        /// <summary>
        /// Pauses the test execution for a short period. This method is sometimes useful when you have
        /// to wait for the page to load.
        /// </summary>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase Pause()
        {
            Thread.Sleep(1000);
            return this;
        }

        /// <summary>
        /// Selects a specified item from the specified drop down list (combo box).
        /// </summary>
        /// <param name="inputControlId">The ID of the input control.</param>
        /// <param name="value">The value to be selected.</param>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase SelectItem(string inputControlId, string value)
        {
            selenium.Select(String.Format(CultureInfo.InvariantCulture,
                                          "xpath=//select[contains(@id,'{0}')]", inputControlId), value);
            return Pause();
        }

        /// <summary>
        /// Stops the Selenium test session.
        /// </summary>
        /// <returns>
        /// The same <see cref="SeleniumTesterBase"/> object.
        /// </returns>
        public SeleniumTesterBase Stop()
        {
            selenium.Stop();
            return this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumTesterBase"/> class
        /// by copying data from an existing <see cref="SeleniumTesterBase"/> object.
        /// </summary>
        /// <param name="testerBase">The existing tester object.</param>
        protected SeleniumTesterBase(SeleniumTesterBase testerBase)
        {
            browserType = testerBase.browserType;
            browserUrl = testerBase.browserUrl;
            selenium = testerBase.selenium;
            seleniumPort = testerBase.seleniumPort;
            seleniumSpeed = testerBase.seleniumSpeed;
            targetUrl = testerBase.targetUrl;
            testMachine = testerBase.testMachine;
        }

        private readonly ISelenium selenium;

        private readonly BrowserType browserType = BrowserType.InternetExplorer;
        private readonly string testMachine;
        private readonly int seleniumPort;
        private readonly string seleniumSpeed;
        private readonly string browserUrl;
        private readonly Uri targetUrl;
    }
}