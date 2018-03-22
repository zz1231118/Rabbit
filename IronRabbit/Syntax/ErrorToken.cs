namespace IronRabbit.Syntax
{
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
}
