using System.Xml.Schema;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    /// <summary>
    /// Unit tests for class <see cref="XmlValidationHelper" />
    /// </summary>
    [TestFixture]
    public class XmlValidationTest
    {
        /// <summary>
        /// Test checks if xml document is valid against xsd schema.
        /// </summary>
        [Test]
        public void XmlValidationWithSchemaTest()
        {
            const string XmlFileName = "../../AccipioTests/Samples/OnlineBankingBusinessActions.xml";
            const string XsdFileName = @"..\..\..\Accipio.Console\AccipioActions.xsd";

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();
            xmlValidationHelper.ValidateXmlDocument(XmlFileName, XsdFileName);
        }

        /// <summary>
        /// Test checks if validator returns exception because of invalid xml document.
        /// </summary>
        [Test, ExpectedException(typeof(XmlSchemaException))]
        public void XmlValidationFailedTest()
        {
            const string XmlFileName = @"..\..\AccipioTests\AccipioActionsIncorrectXml.xml";
            const string XsdFileName = @"..\..\..\Accipio.Console\AccipioActions.xsd";

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();
            xmlValidationHelper.ValidateXmlDocument(XmlFileName, XsdFileName);
        }
    }
}
