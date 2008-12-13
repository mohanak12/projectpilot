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
            const string XmlFileName = @"..\..\..\Data\Samples\BusinessActions.xml";
            const string XsdFileName = @"..\..\..\Data\Samples\AccipioActions.xsd";

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();
            xmlValidationHelper.ValidateXmlDocument(XmlFileName, XsdFileName);

            Assert.AreEqual(xmlValidationHelper.ValidationStatusReport.Length, 0);
        }

        [Test, ExpectedException(typeof(XmlException))]
        public void XmlValidationFailedTest()
        {
            const string XmlFileName = @"..\..\AccipioTests\AccipioActionsIncorrectXml.xml";
            const string XsdFileName = @"..\..\..\Data\Samples\AccipioActions.xsd";

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();
            xmlValidationHelper.ValidateXmlDocument(XmlFileName, XsdFileName);
        }
    }
}
