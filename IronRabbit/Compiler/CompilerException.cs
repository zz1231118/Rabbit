using System.Runtime.Serialization;
using IronRabbit.Syntax;

namespace IronRabbit.Compiler
{
    [Serializable]
    public class CompilerException : Exception
    {
        internal CompilerException(SourceLocation location, string message)
            : base(string.Format("line:{0} column:{1}: {2}", location.Line, location.Column, message))
        { }

        internal CompilerException(SyntaxToken token)
            : base($"token:{token} span:{token.Span}")
        { }

        protected CompilerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
