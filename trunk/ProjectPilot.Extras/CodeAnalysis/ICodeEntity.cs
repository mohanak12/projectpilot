namespace ProjectPilot.Extras.CodeAnalysis
{
    public interface ICodeEntity
    {
        string EntityShortName { get; }
        string EntityFullName { get; }

        void AddAncestor(ICodeEntity ancestor);
    }
}