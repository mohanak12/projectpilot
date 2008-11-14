using System;
using System.Reflection;

namespace ProjectPilot.Extras.CodeAnalysis
{
    public class AssemblyCodeAnalyzer : ICodeAnalyzer
    {
        public AssemblyCodeAnalyzer(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public CodeAnalysisGraph AnalyzeCode()
        {
            CodeAnalysisGraph graph = new CodeAnalysisGraph();

            // first examine all types that belong to the assembly
            foreach(Type type in assembly.GetTypes())
                AnalyzeType(type, graph);

            return graph;
        }

        private ICodeEntity AnalyzeType(Type type, CodeAnalysisGraph graph)
        {
            if (false == ShouldBeIncluded(type))
                return null;

            if (type.IsClass)
            {
                ClassEntity analyzedClass = new ClassEntity(type.Name, type.FullName, type.IsAbstract);
                analyzedClass = (ClassEntity)graph.GetEntity(analyzedClass);

                // skip already analyzed entities
                if (analyzedClass.AnalysisState != CodeEntityAnalysisState.NotAnalyzed)
                    return analyzedClass;

                analyzedClass.AnalysisState = CodeEntityAnalysisState.InAnalysis;

                // find direct ancestor
                if (type.BaseType != null)
                {
                    ICodeEntity ancestor = AnalyzeType(type.BaseType, graph);

                    if (ancestor != null)
                        analyzedClass.AddAncestor(ancestor);
                }

                foreach (Type inheritedInterface in type.GetInterfaces())
                {
                    ICodeEntity ancestor = AnalyzeType(inheritedInterface, graph);

                    if (ancestor != null)
                        analyzedClass.AddAncestor(ancestor);
                }

                foreach (FieldInfo field in type.GetFields(BindingFlags.Instance|BindingFlags.NonPublic
                    ))
                {
                    ICodeEntity association = AnalyzeType(field.FieldType, graph);
                    if (association != null)
                        analyzedClass.AddAssociation(association);
                }

                analyzedClass.AnalysisState = CodeEntityAnalysisState.Analyzed;

                return analyzedClass;
            }
            else if (type.IsInterface)
            {
                InterfaceEntity analyzedInterface = new InterfaceEntity(type.Name, type.FullName);
                analyzedInterface = (InterfaceEntity)graph.GetEntity(analyzedInterface);

                // skip already analyzed entities
                if (analyzedInterface.AnalysisState != CodeEntityAnalysisState.NotAnalyzed)
                    return analyzedInterface;

                analyzedInterface.AnalysisState = CodeEntityAnalysisState.InAnalysis;

                foreach (Type inheritedInterface in type.GetInterfaces())
                {
                    ICodeEntity ancestor = AnalyzeType(inheritedInterface, graph);

                    if (ancestor != null)
                        analyzedInterface.AddAncestor(ancestor);
                }

                analyzedInterface.AnalysisState = CodeEntityAnalysisState.Analyzed;

                return analyzedInterface;
            }

            return null;
        }

        private bool ShouldBeIncluded(Type type)
        {
            return (type.FullName != null && type.FullName.StartsWith("ProjectPilot"));
        }

        private readonly Assembly assembly;
    }
}