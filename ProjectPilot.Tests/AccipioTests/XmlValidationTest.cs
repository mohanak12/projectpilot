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
            string xmlFileName = @"..\..\..\Data\Samples\BusinessActions.xml";
            string xsdFileName = @"..\..\..\Data\Samples\AccipioActions.xsd";

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();
            xmlValidationHelper.ValidateXmlDocument(xmlFileName, xsdFileName);

            Assert.AreEqual(xmlValidationHelper.ValidationStatusReport.Length, 0);
        }

        [Test, ExpectedException(typeof(XmlException)), Ignore("Tests does not accept ExpectedException")]
        public void XmlValidationFailedTest()
        {
            string xmlFileName = @"..\..\AccipioTests\AccipioActionsIncorrectXml.xml";
            string xsdFileName = @"..\..\..\Data\Samples\AccipioActions.xsd";

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();
            xmlValidationHelper.ValidateXmlDocument(xmlFileName, xsdFileName);
        }
    }
}
