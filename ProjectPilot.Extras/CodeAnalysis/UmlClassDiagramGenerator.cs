using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectPilot.Extras.CodeAnalysis
{
    public class UmlClassDiagramGenerator
    {
        public void GenerateDiagram(CodeAnalysisGraph graph, Stream outputStream)
        {
            using (StreamWriter writer = new StreamWriter(outputStream))
            {
                string fontName = "Tahoma";

                writer.WriteLine("digraph");
                writer.WriteLine("{");
                writer.WriteLine(@"node [shape = ""record"" fontsize=""7""]");

                foreach (ICodeEntity entity in graph.Entities.Values)
                {
                    if (entity is ClassEntity)
                    {
                        ClassEntity classEntity = (ClassEntity)entity;
                        writer.WriteLine(
                            "node{0} [label = \"{1}\" fontname=\"{2}\"]", 
                            classEntity.UniqueId, 
                            classEntity.EntityShortName,
                            fontName);

                        foreach (ICodeEntity association in classEntity.Associations)
                        {
                            writer.WriteLine(@"edge [arrowhead = ""none""]");

                            writer.WriteLine(
                                "\"node{0}\" -> \"node{1}\"",
                                association.UniqueId,
                                entity.UniqueId);
                        }
                    }
                    else if (entity is InterfaceEntity)
                    {
                        InterfaceEntity classEntity = (InterfaceEntity)entity;
                        writer.WriteLine(
                            "node{0} [label = \"{1}\" fontname=\"{2} Italic\"]", 
                            classEntity.UniqueId, 
                            classEntity.EntityShortName,
                            fontName);                        
                    }

                    foreach (ICodeEntity ancestor in entity.Ancestors)
                    {
                        if (entity is InterfaceEntity && ancestor is InterfaceEntity)
                            writer.WriteLine(@"edge [arrowhead = ""empty"" style=""dashed"" ]");
                        else
                            writer.WriteLine(@"edge [arrowhead = ""empty""]");

                        writer.WriteLine(
                            "\"node{0}\" -> \"node{1}\"",
                            ancestor.UniqueId,
                            entity.UniqueId);
                    }
                }

                writer.WriteLine("}");
            }
        }
    }
}
