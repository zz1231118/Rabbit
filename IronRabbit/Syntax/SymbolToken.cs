namespace IronRabbit.Syntax
{
    internal class SymbolToken : Token
    {
        private string symbol;

        public SymbolToken(TokenKind kind, string symbol)
            : base(kind)
        {
            this.symbol = symbol;
        }

        public override string Text => symbol;
    }
}
