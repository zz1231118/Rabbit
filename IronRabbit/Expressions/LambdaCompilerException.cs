using System;

namespace IronRabbit.Expressions
{
    [Serializable]
    public class LambdaCompilerException : Exception
    {
        public LambdaCompilerException(string message)
            : base(message)
        { }
    }
}
