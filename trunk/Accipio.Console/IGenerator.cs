namespace Accipio.Console
{
    public interface IGenerator
    {
        bool Parse();
        void Process();
    }
}