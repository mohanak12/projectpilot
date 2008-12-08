using System;
using System.IO;
using System.Text;
using System.Xml;
using MbUnit.Framework;
using ProjectPilot.Framework.CCNet;

namespace ProjectPilot.Tests.Framework.CCNet
{
    [TestFixture]
    public class ProjectStatsLoadingTests
    {
        [Test]
        public void LoadStatisticsTest()
        {
            ICCNetProjectStatisticsPlugIn plugIn = new CCNetProjectStatisticsPlugIn();
            ProjectStatsData data = plugIn.FetchStatistics();

            Assert.AreEqual(data.Builds.Count, 844);
        }

        [Test, ExpectedException(typeof(XmlException))]
        public void IncorrectXmlRootElementTest()
        {
            string xml = "<statisticss><statistic></statistic></statisticss>";

            byte[] bytes = Encoding.ASCII.GetBytes(xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                CCNetProjectStatisticsPlugIn.Load(stream);
            }
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void NotSupportedStatisticsElementTest()
        {
            string xml = "<statistics><statistic></statistic></statistics>";

            byte[] bytes = Encoding.ASCII.GetBytes(xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                CCNetProjectStatisticsPlugIn.Load(stream);
            }
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void NotSupportedIntegrationElementTest()
        {
            string xml = "<statistics><integration><element></element></integration></statistics>";

            byte[] bytes = Encoding.ASCII.GetBytes(xml);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                CCNetProjectStatisticsPlugIn.Load(stream);
            }
        }
    }
}
