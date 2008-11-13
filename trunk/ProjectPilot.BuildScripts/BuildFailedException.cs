using System;

namespace ProjectPilot.BuildScripts
{
    public class BuildFailedException : Exception
    {
        public BuildFailedException()
        {
        }

        public BuildFailedException(string message) : base(message)
        {
        }

        public BuildFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}