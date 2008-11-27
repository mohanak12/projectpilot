#region

using System;
using System.IO;
using Accipio;

#endregion

namespace Accipio.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConsoleApp consoleApp = new ConsoleApp(args);
            consoleApp.Process();
        }
    }
}