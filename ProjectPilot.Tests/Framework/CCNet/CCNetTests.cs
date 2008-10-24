using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using ThoughtWorks.CruiseControl.Remote;
using System.IO;

namespace ProjectPilot.Tests.Framework.CCNet
{
    [TestFixture]
    public class CCNetTests
    {
        [Test]
        public void Test()
        {
            RemoteCruiseManagerFactory factory = new RemoteCruiseManagerFactory();
            string url = string.Format("tcp://kopernik3:21234/CruiseManager.rem");
            ICruiseManager mgr = factory.GetCruiseManager(url);

            string proj = mgr.GetProject("PartnerWeb.Bhwr");
            string stat = mgr.GetStatisticsDocument("PartnerWeb.Bhwr");
            File.WriteAllText("ccnet.stats.xml", stat);
        }
    }
}
