namespace IronRabbit.Syntax
{
    internal class OperatorToken : Token
    {
        private string @operator;
        private byte precedence;

        public OperatorToken(TokenKind kind, string @operator, byte precedence)
            : base(kind)
        {
            this.@operator = @operator;
            this.precedence = precedence;
        }

        public override string Text => @operator;
        public byte Precedence => precedence;
    }
}
