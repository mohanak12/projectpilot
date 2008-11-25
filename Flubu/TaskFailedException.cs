using System;
using System.Collections.Generic;
using System.Text;

namespace Flubu
{
    [Serializable]
    public class TaskFailedException : System.Exception
    {
        public TaskFailedException ()
        {
        }

        public TaskFailedException (string message) : base (message)
        {
        }

        public TaskFailedException (string message, Exception innerException) : base (message, innerException)
        {
        }

        protected TaskFailedException (
            System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext context)
            : base (serializationInfo, context)
        {
        }
    }
}
