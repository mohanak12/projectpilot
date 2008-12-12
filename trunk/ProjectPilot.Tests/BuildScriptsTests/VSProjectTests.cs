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
        [Test,Pending("There are still some bugs in the VSProject code.")]
        public void LoadProject()
        {
            using (Stream stream = File.OpenRead(
                @"..\..\..\Data\Samples\ProjectPilot.Framework.csproj"))
            {
                VSProject project = VSProject.Load(stream);

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
        }
    }
}
