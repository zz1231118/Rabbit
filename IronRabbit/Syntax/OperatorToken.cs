using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronRabbit.Syntax
{
    internal class OperatorToken : Token
    {
        private string _operator;
        private byte _precedence;

        public OperatorToken(TokenKind kind, string @operator, byte precedence)
            : base(kind)
        {
            _operator = @operator;
            _precedence = precedence;
        }

        public override string Text => _operator;
        public byte Precedence => _precedence;
    }
}
