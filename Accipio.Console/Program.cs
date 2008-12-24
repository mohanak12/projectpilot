namespace Accipio.Console
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            ConsoleApp consoleApp = new ConsoleApp(args);
            return consoleApp.Process();
        }
    }
}