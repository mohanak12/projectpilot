using System.Collections.Generic;

namespace ProjectPilot.Extras.CodeAnalysis
{
    public enum CodeEntityAnalysisState
    {
        /// <summary>
        /// NotAnalyzed - Not yet documented
        /// </summary>
        NotAnalyzed,
        
        /// <summary>
        /// InAnalysis - Not yet documented
        /// </summary>
        InAnalysis,
       
        /// <summary>
        /// Analyzed - Not yet documented
        /// </summary>
        Analyzed
    }

    public interface ICodeEntity
    {
        CodeEntityAnalysisState AnalysisState { get; set; }
        
        ICollection<ICodeEntity> Ancestors { get; } 
        
        string EntityShortName { get; }   
        
        string EntityFullName { get; }
        
        int UniqueId { get; set; }

        void AddAncestor(ICodeEntity ancestor);
    }
}