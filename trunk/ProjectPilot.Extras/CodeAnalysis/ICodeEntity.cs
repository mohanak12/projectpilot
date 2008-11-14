using System.Collections.Generic;

namespace ProjectPilot.Extras.CodeAnalysis
{
    public enum CodeEntityAnalysisState
    {
        NotAnalyzed,
        InAnalysis,
        Analyzed
    }

    public interface ICodeEntity
    {
        CodeEntityAnalysisState AnalysisState { get; set; }
        ICollection<ICodeEntity> Ancestors {get;}
        string EntityShortName { get; }
        string EntityFullName { get; }
        int UniqueId { get; set; }

        void AddAncestor(ICodeEntity ancestor);
    }
}