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
                Assert.AreEqual(10, project.References.Count);
            }
        }
    }
}
