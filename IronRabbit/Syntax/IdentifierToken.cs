namespace IronRabbit.Syntax
{
    internal class IdentifierToken : Token
    {
        private string name;

        public IdentifierToken(string name)
            : base(TokenKind.Identifier)
        {
            this.name = name;
        }

        public override string Text => name;
    }
}
