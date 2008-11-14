using System.Collections.Generic;

namespace ProjectPilot.Extras.CodeAnalysis
{
    public class ClassEntity : CodeEntityBase
    {
        public ClassEntity(
            string entityShortName, 
            string entityFullName,
            bool isAbstract) 
            : base(entityShortName, entityFullName)
        {
            this.isAbstract = isAbstract;
        }

        public ICollection<ICodeEntity> Associations
        {
            get {return associations;}
        }

        public bool IsAbstract
        {
            get { return isAbstract; }
        }

        public void AddAssociation(ICodeEntity association)
        {
            associations.Add(association);
        }

        private bool isAbstract;
        private List<ICodeEntity> associations = new List<ICodeEntity>();
    }
}