namespace IronRabbit.Syntax
{
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
}
