using System.Diagnostics;
using System.Globalization;

namespace Flubu.BuildRunner
{
    public class BuildLogger : IBuildLogger
    {
        public void Log(string format, params object[] args)
        {
            string message = string.Format(
                CultureInfo.InvariantCulture,
                format, 
                args);

            Debug.WriteLine(message);
            System.Console.WriteLine(message);
        }
    }
}