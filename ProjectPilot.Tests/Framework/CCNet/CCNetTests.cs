using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using MbUnit.Framework;
using ProjectPilot.Framework.CCNet;
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
            Uri url = new Uri(string.Format(CultureInfo.InvariantCulture, "tcp://firefly:21234/CruiseManager.rem"));
            string projectName = "ProjectPilot";
            ICruiseManager mgr = factory.GetCruiseManager(url.ToString());

            CCNetProjectStatisticsPlugIn plugIn = new CCNetProjectStatisticsPlugIn
                (url, projectName);
            CCNetProjectStatisticsData statisticsPlugIn = plugIn.FetchStatistics();

            string proj = mgr.GetProject(projectName);
            string stat = mgr.GetStatisticsDocument(projectName);
            //File.WriteAllText("ccnet.stats.xml", stat);
        }

        [Test,Ignore]
        public void GraphsTest()
        {
            CCNetProjectStatisticsPlugIn plugIn = new CCNetProjectStatisticsPlugIn();

            List<CCNetProjectStatisticsGraph> graphs = new List<CCNetProjectStatisticsGraph>();

            CCNetProjectStatisticsGraph graph = new CCNetProjectStatisticsGraph();
            graph.AddParameter<TimeSpan>("Build Duration");

            graphs.Add(graph);

            CCNetProjectStatisticsModule module = new CCNetProjectStatisticsModule(plugIn, graphs);

            module.ExecuteTask(null);
        }
    }
}
