using System;
using IronRabbit.Syntax;

namespace IronRabbit.Compiler
{
    [Serializable]
    internal struct TokenWithSpan
    {
        private readonly Token token;
        private readonly IndexSpan span;

        public TokenWithSpan(Token token, IndexSpan span)
        {
            this.token = token;
            this.span = span;
        }

        public IndexSpan Span => span;
        public Token Token => token;
    }
}
