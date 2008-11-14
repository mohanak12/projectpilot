using System.Collections.Generic;

namespace ProjectPilot.Extras.CodeAnalysis
{
    public abstract class CodeEntityBase : ICodeEntity
    {
        public CodeEntityAnalysisState AnalysisState
        {
            get { return analysisState; }
            set { analysisState = value; }
        }

        public ICollection<ICodeEntity> Ancestors
        {
            get { return ancestors; }
        }

        public string EntityFullName
        {
            get { return entityFullName; }
        }

        public string EntityShortName
        {
            get { return entityShortName; }
        }

        public int UniqueId
        {
            get { return uniqueId; }
            set { uniqueId = value; }
        }

        public void AddAncestor (ICodeEntity ancestor)
        {
            ancestors.Add(ancestor);
        }

        protected CodeEntityBase(
            string entityShortName,
            string entityFullName)
        {
            this.entityShortName = entityShortName;
            this.entityFullName = entityFullName;
        }

        private CodeEntityAnalysisState analysisState = CodeEntityAnalysisState.NotAnalyzed;
        private List<ICodeEntity> ancestors = new List<ICodeEntity> ();
        private readonly string entityFullName;
        private string entityShortName;
        private int uniqueId;
    }
}