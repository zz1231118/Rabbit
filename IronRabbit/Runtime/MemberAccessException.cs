using System;
using System.Runtime.Serialization;

namespace IronRabbit.Runtime
{
    [Serializable]
    public class MemberAccessException : SystemException
    {
        public MemberAccessException()
        { }
        public MemberAccessException(string message)
            : base(message)
        { }
        public MemberAccessException(string message, Exception innerException)
            : base(message, innerException)
        { }
        protected MemberAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
