using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.IO;
using Flubu.Builds.VSSolutionBrowsing;

namespace ProjectPilot.Tests.BuildScriptsTests
{
    [TestFixture]
    public class VSProjectTests
    {
        [Test]
        public void TestVSProject1()
        {
            VSProject project = VSProject.Load (@"..\..\..\Data\Samples\ProjectPilot.Framework.csproj");

            Assert.AreEqual(48, project.CompileItems.Count);
            Assert.AreEqual(2, project.Configurations.Count);
            Assert.AreEqual(9, project.References.Count);
            Assert.AreEqual(14, project.Properties.Count);

            Assert.AreEqual(@"Subversion\SubversionHistoryFacility.cs", project.CompileItems[47].Compile);
            Assert.AreEqual(" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ", project.Configurations[0].Condition);
            Assert.AreEqual("System.Data", project.References[3].Include);
            Assert.AreEqual("OutputType", project.Properties.ElementAt(5).Key);
            Assert.AreEqual("Properties", project.Properties.ElementAt(6).Value);
        }
 
        [Test]
        public void TestVSProject2()
        {
            VSProject project = VSProject.Load (@"..\..\..\Data\Samples\ProjectPilot.Portal.csproj");

            Assert.AreEqual(24, project.CompileItems.Count);
            Assert.AreEqual(2, project.Configurations.Count);
            Assert.AreEqual(16, project.References.Count);
            Assert.AreEqual(13, project.Properties.Count);

            Assert.AreEqual(@"Views\Shared\Site.Master.designer.cs", project.CompileItems[23].Compile);
            Assert.AreEqual(" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ", project.Configurations[1].Condition);
            Assert.AreEqual("System.Web.Routing, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL", project.References[6].Include);
            Assert.AreEqual("HintPath", project.References[6].ReferenceAttributes.ElementAt(1).Key);
            Assert.AreEqual("True", project.References[6].ReferenceAttributes.ElementAt(3).Value);
            Assert.AreEqual("ProjectGuid", project.Properties.ElementAt(4).Key);
            Assert.AreEqual("{603c0e0b-db56-11dc-be95-000d561079b0};{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}", project.Properties.ElementAt(5).Value);
        }
    }
}
