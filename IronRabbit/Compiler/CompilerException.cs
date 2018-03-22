using System;

namespace IronRabbit.Compiler
{
    [Serializable]
    public class CompilerException : Exception
    {
        public CompilerException(SourceLocation location, string message)
            : base(string.Format("line:{0} column:{1}: {2}", location.Line, location.Column, message))
        { }
    }
}
