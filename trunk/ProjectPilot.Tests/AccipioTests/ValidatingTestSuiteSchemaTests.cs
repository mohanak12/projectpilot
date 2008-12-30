using System;
using System.IO;
using System.Xml.Schema;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// This is a set of tests that check how the test suite XML schema was generated.
    /// By using the generated schema to validate some typical examples of test suites
    /// we test if the schema was generated properly.
    /// </summary>
    [TestFixture]
    public class ValidatingTestSuiteSchemaTests
    {
        /// <summary>
        /// Makes sure the schema does not allow unknown steps in test cases.
        /// </summary>
        [Test]
        public void WrongAction()
        {
            PrepareTestSuiteXmlFile(@"
<suite id='Banking' runner='OnlineBanking' namespace='OnlineBankingNamespace' xmlns='http://fikus'>
    <description>Dummy description.</description>
    <case id='case1'>
        <description>Dummy description.</description>
        <steps>
            <sds/>
        </steps>
    </case>
</suite>
");
            AssertTestSuiteXmlFileValidity(false);
        }

        /// <summary>
        /// Makes sure the schema allows test cases to be without steps.
        /// </summary>
        [Test]
        public void NoSteps()
        {
            PrepareTestSuiteXmlFile(@"
<suite id='Banking' runner='OnlineBanking' namespace='OnlineBankingNamespace' xmlns='http://fikus'>
    <description>Dummy description.</description>
    <case id='case1'>
        <description>Dummy description.</description>
        <tags>R15</tags>
    </case>
</suite>
");
            AssertTestSuiteXmlFileValidity(true);
        }

        /// <summary>
        /// Makes sure the schema allows test cases without tags.
        /// </summary>
        [Test]
        public void NoTags()
        {
            PrepareTestSuiteXmlFile(@"
<suite id='Banking' runner='OnlineBanking' namespace='OnlineBankingNamespace' xmlns='http://fikus'>
    <description>Dummy description.</description>
    <case id='case1'>
        <description>Dummy description.</description>
    </case>
</suite>
");
            AssertTestSuiteXmlFileValidity(true);
        }

        /// <summary>
        /// Makes sure the schema does not allow test cases without descriptions.
        /// </summary>
        [Test]
        public void NoCaseDescription()
        {
            PrepareTestSuiteXmlFile(@"
<suite id='Banking' runner='OnlineBanking' namespace='OnlineBankingNamespace' xmlns='http://fikus'>
    <description>Dummy description.</description>
    <case id='case1'>
        <steps/>
    </case>
</suite>
");
            AssertTestSuiteXmlFileValidity(false);
        }

        /// <summary>
        /// Makes sure the schema enforces that the action without parameters should be an empty XML element
        /// (without the inner content).
        /// </summary>
        [Test]
        public void StepWithoutParametersShouldBeEmpty()
        {
            PrepareTestSuiteXmlFile(
                @"
<suite id='Banking' runner='OnlineBanking' namespace='OnlineBankingNamespace' xmlns='http://fikus'>
    <description>Dummy description.</description>
    <case id='case1'>
        <description>Dummy description.</description>
        <steps>
            <ActionNoParameters>bla bla</ActionNoParameters>
        </steps>
    </case>
</suite>
");

            AssertTestSuiteXmlFileValidity(false);
        }

        /// <summary>
        /// Makes sure the schema enforces that the action with parameters should not have text node.
        /// </summary>
        [Test]
        public void StepWithParametersShouldNotHaveTextEmpty()
        {
            PrepareTestSuiteXmlFile(
                @"
<suite id='Banking' runner='OnlineBanking' namespace='OnlineBankingNamespace' xmlns='http://fikus'>
    <description>Dummy description.</description>
    <case id='case1'>
        <description>Dummy description.</description>
        <steps>
            <Action1Parameter parameter='1'>dfsfsd</Action1Parameter>
        </steps>
    </case>
</suite>
");

            AssertTestSuiteXmlFileValidity(false);
        }

        /// <summary>
        /// Makes sure the schema allows two different forms for actions with a single parameter.
        /// </summary>
        [Test]
        public void StepWithSingleParameter()
        {
            PrepareTestSuiteXmlFile(
                @"
<suite id='Banking' runner='OnlineBanking' namespace='OnlineBankingNamespace' xmlns='http://fikus'>
    <description>Dummy description.</description>
    <case id='case1'>
        <description>Dummy description.</description>
        <steps>
            <Action1Parameter parameter='1'/>
        </steps>
    </case>
</suite>
");

            AssertTestSuiteXmlFileValidity(true);
        }

        [SetUp]
        public void Setup()
        {
            PrepareBusinessActionsFile();
        }

        private void AssertTestSuiteXmlFileValidity(bool shouldBeValid)
        {
            try
            {
                // first generate the schema
                TestSuiteSchemaGeneratorCommand cmd = new TestSuiteSchemaGeneratorCommand(null);
                cmd.AccipioDirectory = string.Empty;
                string[] arguments = new string[] { "baschema", BusinessActionsXmlFile, "http://fikus" };
                Assert.AreSame(cmd, cmd.ParseArguments(arguments));
            
                cmd.ProcessCommand();
                string testSuiteSchemaFile = cmd.OutputFile;

                // now validate our test suite with the generated schema
                XmlValidationHelper helper = new XmlValidationHelper();
                helper.ValidateXmlDocument(TestSuiteXmlFile, testSuiteSchemaFile);
                Assert.IsTrue(shouldBeValid);
            }
            catch (XmlSchemaException ex)
            {
                Assert.IsFalse(shouldBeValid, "Validation failed: '{0}'", ex.Message);
            }
        }

        private void PrepareBusinessActionsFile()
        {
            const string BusinessActionsFileContents = @"
<actions xmlns='http://projectpilot/AccipioActions.xsd'>
    <action id='ActionNoParameters'>
       <description>Description1</description>
    </action>

    <action id='Action1Parameter'>
       <description>Description1</description>
       <parameter name='parameter' type='int'/>
    </action>

    <action id='Action2Parameters'>
       <description>Description1</description>
       <parameter name='parameter1' type='int'/>
       <parameter name='parameter2' type='int'/>
    </action>
</actions>
";
            File.WriteAllText(
                BusinessActionsXmlFile,
                BusinessActionsFileContents);
        }

        private void PrepareTestSuiteXmlFile (string fileContents)
        {
            File.WriteAllText(TestSuiteXmlFile, fileContents);
        }

        private const string BusinessActionsXmlFile = "BusinessActionsXmlFile.xml";
        private const string TestSuiteXmlFile = "TestSuiteXmlFile.xml";
    }
}
