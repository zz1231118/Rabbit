namespace IronRabbit.Compiler
{
    internal abstract class Token
    {
        private TokenKind _kind;

        protected Token(TokenKind kind)
        {
            _kind = kind;
        }

        public abstract string Text { get; }

        public TokenKind Kind => _kind;
        public virtual decimal Value
        {
            get => throw new System.NotSupportedException();
        }
        
        public static Token Error(string message)
        {
            return new ErrorToken(message);
        }
        public static Token Operator(TokenKind kind, string @operator, byte precedence)
        {
            return new OperatorToken(kind, @operator, precedence);
        }
        public static Token Symbol(TokenKind kind, string symbol)
        {
            return new SymbolToken(kind, symbol);
        }
        public static Token Constant(decimal value)
        {
            return new ConstantToken(value);
        }
        public static Token Comment(string text)
        {
            return new CommentToken(text);
        }
        public static Token Identifier(string name)
        {
            return new IdentifierToken(name);
        }

        public override string ToString()
        {
            return Text.ToString();
        }

        internal class ErrorToken : Token
        {
            private string _message;

            public ErrorToken(string message)
                : base(TokenKind.Error)
            {
                _message = message;
            }

            public override string Text => _message;
            public string Message => _message;
        }
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
        internal class SymbolToken : Token
        {
            private string _symbol;

            public SymbolToken(TokenKind kind, string symbol)
                : base(kind)
            {
                _symbol = symbol;
            }

            public override string Text => _symbol;
        }
        internal class ConstantToken : Token
        {
            private decimal _value;

            public ConstantToken(decimal value)
                : base(TokenKind.Constant)
            {
                _value = value;
            }

            public override string Text => _value.ToString();
            public override decimal Value => _value;
        }
        internal class CommentToken : Token
        {
            private string _comment;

            public CommentToken(string comment)
                : base(TokenKind.Comment)
            {
                _comment = comment;
            }

            public override string Text => _comment;
        }
        internal class IdentifierToken : Token
        {
            private string _name;

            public IdentifierToken(string name)
                : base(TokenKind.Identifier)
            {
                _name = name;
            }

            public override string Text => _name;
        }
    }
}
