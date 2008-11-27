using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu
{
    [Serializable]
    public class RunnerFailedException : System.Exception
    {
        public RunnerFailedException ()
        {
        }

        public RunnerFailedException (string message) : base (message)
        {
        }

        public RunnerFailedException (string message, Exception innerException) : base (message, innerException)
        {
        }

        protected RunnerFailedException (
            System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext context)
            : base (serializationInfo, context)
        {
        }
    }
}
