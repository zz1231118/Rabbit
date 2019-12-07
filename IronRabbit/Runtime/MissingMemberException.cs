using System;
using System.Runtime.Serialization;

namespace IronRabbit.Runtime
{
    [Serializable]
    public class MissingMemberException : MemberAccessException
    {
        public MissingMemberException()
        { }

        public MissingMemberException(string message)
            : base(message)
        { }

        public MissingMemberException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected MissingMemberException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
