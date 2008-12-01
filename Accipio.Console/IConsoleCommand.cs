namespace Accipio.Console
{
    public interface IConsoleCommand
    {
        IConsoleCommand ParseArguments(string[] args);

        void ProcessCommand();
    }
}