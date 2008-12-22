using System.Collections.Generic;
using System.Linq;
using Flubu.Builds.VSSolutionBrowsing;
using MbUnit.Framework;

namespace ProjectPilot.Tests.BuildScriptsTests
{
    [TestFixture]
    public class VSProjectTests
    {
        [Test]
        public void TestParsingVSProjectFile1()
        {
            VSProject project = VSProject.Load (@"..\..\..\Data\Samples\ProjectPilot.Framework.csproj");

            IList<VSProjectItem> compileItems = project.GetSingleTypeItems(VSProjectItem.CompileItem);
            IList<VSProjectItem> referenceItems = project.GetSingleTypeItems(VSProjectItem.Reference);
            
            Assert.AreEqual(48, compileItems.Count);
            Assert.AreEqual(2, project.Configurations.Count);
            Assert.AreEqual(9, referenceItems.Count);
            Assert.AreEqual(14, project.Properties.Count);

            Assert.AreEqual(
                            @"Subversion\SubversionHistoryFacility.cs",
                            project.GetSingleTypeItems("CompileItem").ElementAt(47).Item);//comp
            
            Assert.AreEqual(
                            " '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ",
                            project.Configurations[0].Condition);
            
            Assert.AreEqual("System.Data", project.Items[3].Item);//ref
            Assert.AreEqual("System.Xml", project.GetSingleTypeItems("Reference").ElementAt(6).Item);//ref

            Assert.AreEqual("OutputType", project.Properties.ElementAt(5).Key);
            Assert.AreEqual("Properties", project.Properties.ElementAt(6).Value);
        }
 
        [Test]
        public void TestParsingVSProjectFile2()
        {
            VSProject project = VSProject.Load (@"..\..\..\Data\Samples\ProjectPilot.Portal.csproj");

            IList<VSProjectItem> compileItems = project.GetSingleTypeItems(VSProjectItem.CompileItem);
            IList<VSProjectItem> referenceItems = project.GetSingleTypeItems(VSProjectItem.Reference);

            Assert.AreEqual(24, compileItems.Count);//compile
            Assert.AreEqual(2, project.Configurations.Count);
            Assert.AreEqual(16, referenceItems.Count);//ref
            Assert.AreEqual(13, project.Properties.Count);

            Assert.AreEqual(@"Views\Shared\Site.Master.designer.cs", compileItems.ElementAt(23).Item);//comp

            Assert.AreEqual(" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ", project.Configurations[1].Condition);
            Assert.AreEqual("System.Web.Routing, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL", project.Items[6].Item);//ref
            Assert.AreEqual("HintPath", referenceItems.ElementAt(6).ItemAttributes.ElementAt(1).Key);//ref
            Assert.AreEqual("HintPath", project.Items[6].ItemAttributes.ElementAt(1).Key);//ref
            Assert.AreEqual("True", project.Items[6].ItemAttributes.ElementAt(3).Value);//ref
            Assert.AreEqual("ProjectGuid", project.Properties.ElementAt(4).Key);
            Assert.AreEqual("{603c0e0b-db56-11dc-be95-000d561079b0};{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}", project.Properties.ElementAt(5).Value);
        }
    }
}
