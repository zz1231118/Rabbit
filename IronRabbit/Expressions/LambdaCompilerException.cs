using System.Runtime.Serialization;

namespace IronRabbit.Expressions
{
    [Serializable]
    public class LambdaCompilerException : Exception
    {
        protected LambdaCompilerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public LambdaCompilerException(string message)
            : base(message)
        { }
    }
}
