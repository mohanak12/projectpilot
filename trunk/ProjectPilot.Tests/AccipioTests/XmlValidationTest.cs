using System.Xml;
using Accipio.Console;
using MbUnit.Framework;

namespace ProjectPilot.Tests.AccipioTests
{
    [TestFixture]
    public class XmlValidationTest
    {
        [Test]
        public void XmlValidationWithSchemaTest()
        {
            const string xmlFileName = @"..\..\..\Data\Samples\BusinessActions.xml";
            const string xsdFileName = @"..\..\..\Data\Samples\AccipioActions.xsd";

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();
            xmlValidationHelper.ValidateXmlDocument(xmlFileName, xsdFileName);

            Assert.AreEqual(xmlValidationHelper.ValidationStatusReport.Length, 0);
        }

        [Test, ExpectedException(typeof(XmlException))]
        public void XmlValidationFailedTest()
        {
            const string xmlFileName = @"..\..\AccipioTests\AccipioActionsIncorrectXml.xml";
            const string xsdFileName = @"..\..\..\Data\Samples\AccipioActions.xsd";

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();
            xmlValidationHelper.ValidateXmlDocument(xmlFileName, xsdFileName);
        }
    }
}
