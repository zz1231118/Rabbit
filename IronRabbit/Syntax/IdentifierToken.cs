namespace IronRabbit.Syntax
{
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
