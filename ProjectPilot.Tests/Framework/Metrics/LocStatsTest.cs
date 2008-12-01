using System.IO;
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
            LocStatsData data = locStats.CountLocString(
@"// this is one comment
/// this is another comment
this is a statement

statement // with comment");

            Assert.AreEqual(3, data.Cloc);
            Assert.AreEqual(1, data.Eloc);
            Assert.AreEqual(5, data.Sloc);
        }

        [Test, Ignore]
        public void Test2()
        {
            ILocStats locStats = new LocStats();
            LocStatsData data = locStats.CountLocString(
@"// this is one comment
/// this is another comment




for(i=0;i<x;i++)
{
     sdf; //iteration
     sdf;
}
///xml comment

this is a statement //comment


statement // with comment"
);

            Assert.AreEqual(6, data.Cloc);
            Assert.AreEqual(7, data.Eloc);
            Assert.AreEqual(17, data.Sloc);
        }

        [Test,Pending("Have to use a static C# file")]
        public void Test3()
        {
            ILocStats locStats = new LocStats();
            LocStatsData data = locStats.CountLocFile(@"c:\PilotProject\Data\Samples\LocSample.cs");

            Assert.AreEqual(8, data.Cloc); //TODO: Wrong!!! comment inside string should not be counted
            Assert.AreEqual(9, data.Eloc);
            Assert.AreEqual(56, data.Sloc);
        }

        [Test,Ignore]
        public void Test4()
        {
            ILocStats locStats = new LocStats();

            Stream stream = File.OpenRead(@"c:\PilotProject\Data\Samples\LocSample.cs");
            
            LocStatsData data = locStats.CountLocString(stream);

            Assert.AreEqual(8, data.Cloc);
            Assert.AreEqual(9, data.Eloc);
            Assert.AreEqual(56, data.Sloc);
        }
    }
}
