namespace Accipio.Console
{
    public interface IGenerator
    {
        void Parse(string[] args);
        void Process();
    }
}