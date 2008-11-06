using System.Globalization;
using System.IO;
using MbUnit.Framework;
using ThoughtWorks.CruiseControl.Remote;

namespace ProjectPilot.Tests.Framework.CCNet
{
    [TestFixture]
    public class CCNetTests
    {
        [Test]
        public void Test()
        {
            RemoteCruiseManagerFactory factory = new RemoteCruiseManagerFactory();
            string url = string.Format(CultureInfo.InvariantCulture, "tcp://kopernik3:21234/CruiseManager.rem");
            ICruiseManager mgr = factory.GetCruiseManager(url);

            string proj = mgr.GetProject("PartnerWeb.Bhwr");
            string stat = mgr.GetStatisticsDocument("PartnerWeb.Bhwr");
            File.WriteAllText("ccnet.stats.xml", stat);
        }
    }
}
