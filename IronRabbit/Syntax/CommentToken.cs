namespace IronRabbit.Syntax
{
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
}
