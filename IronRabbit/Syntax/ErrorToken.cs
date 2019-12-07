namespace IronRabbit.Syntax
{
    internal class ErrorToken : Token
    {
        private string message;

        public ErrorToken(string message)
            : base(TokenKind.Error)
        {
            this.message = message;
        }

        public override string Text => message;

        public string Message => message;
    }
}
