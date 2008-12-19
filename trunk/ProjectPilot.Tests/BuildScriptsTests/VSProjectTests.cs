using System.Linq;
using Flubu.Builds.VSSolutionBrowsing;
using MbUnit.Framework;

namespace ProjectPilot.Tests.BuildScriptsTests
{
    [TestFixture]
    public class VSProjectTests
    {
        [Test, Pending]
        public void TestParsingVSProjectFile1()
        {
            VSProject project = VSProject.Load (@"..\..\..\Data\Samples\ProjectPilot.Framework.csproj");

            Assert.AreEqual(48, project.Items.Count);//compile
            Assert.AreEqual(2, project.Configurations.Count);
            Assert.AreEqual(9, project.Items.Count);//ref
            Assert.AreEqual(14, project.Properties.Count);

            Assert.AreEqual(@"Subversion\SubversionHistoryFacility.cs", project.Items[47].Item);//comp
            Assert.AreEqual(" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ", project.Configurations[0].Condition);
            Assert.AreEqual("System.Data", project.Items[3].Item);//ref
            Assert.AreEqual("OutputType", project.Properties.ElementAt(5).Key);
            Assert.AreEqual("Properties", project.Properties.ElementAt(6).Value);
        }
 
        [Test, Pending]
        public void TestParsingVSProjectFile2()
        {
            VSProject project = VSProject.Load (@"..\..\..\Data\Samples\ProjectPilot.Portal.csproj");

            Assert.AreEqual(24, project.Items.Count);//compile
            Assert.AreEqual(2, project.Configurations.Count);
            Assert.AreEqual(16, project.Items.Count);//ref
            Assert.AreEqual(13, project.Properties.Count);

            Assert.AreEqual(@"Views\Shared\Site.Master.designer.cs", project.Items[23].Item);//comp
            Assert.AreEqual(" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ", project.Configurations[1].Condition);
            Assert.AreEqual("System.Web.Routing, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL", project.Items[6].Item);//ref
            Assert.AreEqual("HintPath", project.Items[6].ItemAttributes.ElementAt(1).Key);//ref
            Assert.AreEqual("True", project.Items[6].ItemAttributes.ElementAt(3).Value);//ref
            Assert.AreEqual("ProjectGuid", project.Properties.ElementAt(4).Key);
            Assert.AreEqual("{603c0e0b-db56-11dc-be95-000d561079b0};{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}", project.Properties.ElementAt(5).Value);
        }
    }
}
