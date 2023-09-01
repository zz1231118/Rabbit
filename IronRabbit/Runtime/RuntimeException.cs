﻿using System.Runtime.Serialization;

namespace IronRabbit.Runtime
{
    [Serializable]
    public class RuntimeException : Exception
    {
        public RuntimeException()
        { }

        public RuntimeException(string message)
            : base(message)
        { }

        public RuntimeException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected RuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
