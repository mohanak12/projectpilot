using System.Collections.Generic;

namespace ProjectPilot.Extras.CodeAnalysis
{
    public abstract class CodeEntityBase : ICodeEntity
    {
        public string EntityFullName
        {
            get { return entityFullName; }
        }

        public string EntityShortName
        {
            get { return entityShortName; }
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

        private List<ICodeEntity> ancestors = new List<ICodeEntity> ();
        private readonly string entityFullName;
        private string entityShortName;
    }
}