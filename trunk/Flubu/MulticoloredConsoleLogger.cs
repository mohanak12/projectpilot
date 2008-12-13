using System;
using System.IO;

namespace Flubu
{
    /// <summary>
    /// A standard multi-colored console output for Flubu.
    /// </summary>
    public class MulticoloredConsoleLogger : IFlubuLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MulticoloredConsoleLogger"/> class
        /// using the specified text writer.
        /// </summary>
        /// <param name="writer">The text writer to use to write log messages.</param>
        public MulticoloredConsoleLogger(TextWriter writer)
        {
            this.writer = writer;

            if (IsConsoleOutput)
            {
                // remember user's standard colors so he doesn't get pissed off 
                // if we left the console switched to pink after the build
                defaultForegroundColor = Console.ForegroundColor;
                defaultBackgroundColor = Console.BackgroundColor;

                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        public void LogError(string message)
        {
            WriteLine(ConsoleColor.Red, message);
        }

        public void LogError(string format, params object[] args)
        {
            WriteLine(ConsoleColor.Red, format, args);
        }

        public void LogMessage(string message)
        {
            WriteLine (ConsoleColor.DarkGray, message);
        }

        public void LogMessage(string format, params object[] args)
        {
            WriteLine (ConsoleColor.DarkGray, format, args);
        }

        public void LogRunnerFinished(bool success, TimeSpan buildDuration)
        {
            // reset the depth counter to make the build report non-indented
            executionDepthCounter = 0;

            WriteLine (ConsoleColor.DarkGray, String.Empty);
            if (success)
                WriteLine (ConsoleColor.Green, "BUILD SUCCESSFUL");
            else
                WriteLine(ConsoleColor.Red, "BUILD FAILED");

            WriteLine(ConsoleColor.White, "Build duration: {0} ({1} seconds)", buildDuration, (int)buildDuration.TotalSeconds);
        }

        public void LogTargetFinished()
        {
            executionDepthCounter--;
        }

        public void LogTargetStarted(string targetName)
        {
            WriteLine(ConsoleColor.White, String.Empty);
            WriteLine (ConsoleColor.White, "{0}:", targetName);
            executionDepthCounter++;
        }

        public void LogTaskFinished()
        {
            executionDepthCounter--;
        }

        public void LogTaskStarted(string taskDescription)
        {
            WriteLine (ConsoleColor.Gray, "TASK: {0}", taskDescription);
            executionDepthCounter++;
        }

        /// <summary>
        /// Gets a value indicating whether this logger logs to the <see cref="Console.Out"/>.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance uses console output; otherwise, <c>false</c>.
        /// </value>
        protected bool IsConsoleOutput
        {
            get { return ReferenceEquals(writer, System.Console.Out); }
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">If <code>false</code>, cleans up native resources. 
        /// If <code>true</code> cleans up both managed and native resources</param>
        protected virtual void Dispose (bool disposing)
        {
            if (false == disposed)
            {
                if (disposing)
                {
                    // restore user's standard colors
                    Console.ForegroundColor = defaultForegroundColor;
                    Console.BackgroundColor = defaultBackgroundColor;

                    writer.Dispose();
                }

                disposed = true;
            }
        }

        private void WriteLine (ConsoleColor foregroundColor, string message)
        {
            if (IsConsoleOutput)
                Console.ForegroundColor = foregroundColor;

            string indentation = new string (' ', executionDepthCounter * 3);
            writer.Write (indentation);
            writer.WriteLine (message);
        }

        private void WriteLine(ConsoleColor foregroundColor, string format, params object[] args)
        {
            if (IsConsoleOutput)
                Console.ForegroundColor = foregroundColor;

            string indentation = new string (' ', executionDepthCounter * 3);
            writer.Write(indentation);
            writer.WriteLine(format, args);
        }

        private bool disposed;
        private int executionDepthCounter;
        private readonly TextWriter writer;
        private ConsoleColor defaultForegroundColor;
        private ConsoleColor defaultBackgroundColor;
    }
}