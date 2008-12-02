namespace Accipio.Console
{
    /// <summary>
    /// Represents a command provided by the Accipio.Console.
    /// </summary>
    public interface IConsoleCommand
    {
        /// <summary>
        /// Returns the first <see cref="IConsoleCommand"/> in the command chain 
        /// which can understand the provided command-line arguments.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>The first <see cref="IConsoleCommand"/> which can understand the provided command-line arguments
        /// or <c>null</c> if none of the console commands can understand them.</returns>
        IConsoleCommand ParseArguments(string[] args);

        /// <summary>
        /// Processes the command.
        /// </summary>
        void ProcessCommand();
    }
}