using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using ProjectPilot.Framework.Metrics;

namespace ProjectPilot.Tests.Framework.Metrics
{
    [TestFixture]
    public class LocStatsTest
    {
        [Test]
        public void Test1()
        {
            ILocStats locStats = new LocStats();
            LocStatsData data = locStats.CountLoc(
@"// this is one comment
/// this is another comment
this is a statement

statement // with comment");

            Assert.AreEqual(3, data.Cloc);
            Assert.AreEqual(1, data.Eloc);
            Assert.AreEqual(5, data.Sloc);
        }
    }
}
