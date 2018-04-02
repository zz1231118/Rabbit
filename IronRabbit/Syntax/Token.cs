namespace IronRabbit.Syntax
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
        public virtual double Value
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
        public static Token Constant(double value)
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
    }
}
