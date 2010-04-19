namespace Flubu.Packaging
{
    public class StandardPackageDef : CompositeFilesSource, IPackageDef
    {
        public StandardPackageDef() : base(string.Empty)
        {
        }

        public StandardPackageDef(string id) : base(id)
        {
        }
    }
}