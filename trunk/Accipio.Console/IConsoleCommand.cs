using System.Collections.Generic;

namespace Accipio.Console
{
    /// <summary>
    /// Represents a command provided by the Accipio.Console.
    /// </summary>
    public interface IConsoleCommand
    {
        string CommandDescription { get; }

        string CommandName { get; }

        int Execute(IEnumerable<string> args);
        
        void ShowHelp();
    }
}