using System.Globalization;
using System.IO;
using Castle.Core;
using Castle.Windsor;

namespace ProjectPilot.Tests
{
    public sealed class WindsorContainerGraphs
    {
        public static void GenerateDependencyGraph(
            IWindsorContainer windsorContainer,
            Stream outputStream)
        {
            using (StreamWriter writer = new StreamWriter(outputStream))
            {
                writer.WriteLine("digraph");
                writer.WriteLine("{");
                writer.WriteLine("node [shape=component]");

                foreach (GraphNode node in windsorContainer.Kernel.GraphNodes)
                {
                    ComponentModel dependingNode = (ComponentModel)node;

                    if (dependingNode.Adjacencies.Length > 0)
                    {
                        int a = 0; 
                        a++;
                    }

                    string dependingNodeString = ContructNodeName(dependingNode);
                    writer.WriteLine("\"{0}\"", dependingNodeString);

                    foreach (GraphNode depender in node.Dependers)
                    {
                        ComponentModel dependerNode = (ComponentModel)depender;

                        string dependerNodeString = ContructNodeName(dependerNode);

                        writer.WriteLine(
                            "\"{0}\" -> \"{1}\"",
                            dependerNodeString,
                            dependingNodeString);
                    }
                }

                writer.WriteLine("}");
            }
        }

        public static void GenerateDependencyGraph(
            IWindsorContainer windsorContainer,
            string dotFileName)
        {
            using (Stream stream = File.Open(dotFileName, FileMode.Create))
            {
                GenerateDependencyGraph(windsorContainer, stream);
            }
        }

        private WindsorContainerGraphs()
        {
        }

        private static string ContructNodeName(ComponentModel dependerNode)
        {
            // [label=< <TABLE BORDER="0" CELLBORDER="0" CELLSPACING="0">  <TR><TD>top</TD></TR><TR><TD><font point-size="10">bottom</font></TD></TR> </TABLE>>];
            string dependerNodeString;
            if (dependerNode.Service != dependerNode.Implementation)
                dependerNodeString = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}\\n({1})",
                    dependerNode.Implementation.Name,
                    dependerNode.Service.Name);
            else
                dependerNodeString = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}",
                    dependerNode.Implementation.Name);
            return dependerNodeString;
        }
    }
}