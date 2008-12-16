using System.Xml;
using System.Xml.Schema;
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
            const string XsdFileName = @"..\..\..\Accipio.Console\AccipioActions.xsd";

            XmlValidationHelper xmlValidationHelper = new XmlValidationHelper();
            xmlValidationHelper.ValidateXmlDocument(XmlFileName, XsdFileName);
        }

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
