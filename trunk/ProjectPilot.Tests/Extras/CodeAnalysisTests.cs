using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using ProjectPilot.Extras.CodeAnalysis;
using ProjectPilot.Framework;

namespace ProjectPilot.Tests.Extras
{
    [TestFixture]
    public class CodeAnalysisTests
    {
        [Test]
        public void Test()
        {
            AssemblyCodeAnalyzer analyzer = new AssemblyCodeAnalyzer(typeof(Project).Assembly);
            CodeAnalysisGraph graph = analyzer.AnalyzeCode();

            using (Stream stream = File.Open("test.dot", FileMode.Create))
            {
                UmlClassDiagramGenerator generator = new UmlClassDiagramGenerator();
                generator.GenerateDiagram(graph, stream);
            }
        }
    }
}
