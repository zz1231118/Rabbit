using System;

namespace IronRabbit.Compiler
{
    [Serializable]
    internal struct TokenWithSpan
    {
        private readonly Token _token;
        private readonly IndexSpan _span;

        public TokenWithSpan(Token token, IndexSpan span)
        {
            _token = token;
            _span = span;
        }

        public IndexSpan Span => _span;
        public Token Token => _token;
    }
}
