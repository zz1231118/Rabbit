using System.Runtime.Serialization;

namespace IronRabbit.Runtime
{
    [Serializable]
    public class SystemException : Exception
    {
        public SystemException()
        { }

        public SystemException(string message)
            : base(message)
        { }

        public SystemException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected SystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
