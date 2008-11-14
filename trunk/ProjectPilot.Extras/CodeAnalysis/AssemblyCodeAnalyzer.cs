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

            foreach(Type type in assembly.GetTypes())
            {
                if (type.IsClass)
                {
                    ClassEntity analyzedClass = new ClassEntity(type.Name, type.FullName, type.IsAbstract);

                    // find direct ancestor
                    if (type.BaseType != null)
                    {
                        // should this type be included in the graph?
                        if (ShouldBeIncluded (type.BaseType))
                        {
                            ClassEntity ancestor = new ClassEntity (
                                type.BaseType.Name, 
                                type.BaseType.FullName, 
                                type.BaseType.IsAbstract);
                        }
                    }
                }
            }

            throw new NotImplementedException();
        }

        private bool ShouldBeIncluded(Type type)
        {
            throw new NotImplementedException();
        }

        private readonly Assembly assembly;
    }
}