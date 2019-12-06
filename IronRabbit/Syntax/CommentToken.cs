namespace IronRabbit.Syntax
{
    internal class CommentToken : Token
    {
        private string comment;

        public CommentToken(string comment)
            : base(TokenKind.Comment)
        {
            this.comment = comment;
        }

        public override string Text => comment;
    }
}
