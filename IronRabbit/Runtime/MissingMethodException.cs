using System;
using System.Runtime.Serialization;

namespace IronRabbit.Runtime
{
    [Serializable]
    public class MissingMethodException : MissingMemberException
    {
        public MissingMethodException()
        { }
        public MissingMethodException(string message)
            : base(message)
        { }
        public MissingMethodException(string message, Exception innerException)
            : base(message, innerException)
        { }
        protected MissingMethodException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
