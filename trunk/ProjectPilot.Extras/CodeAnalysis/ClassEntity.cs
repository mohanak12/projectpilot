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

        public bool IsAbstract
        {
            get { return isAbstract; }
        }

        private bool isAbstract;
    }
}