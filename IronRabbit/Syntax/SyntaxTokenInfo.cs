using IronRabbit.Expressions;

namespace IronRabbit.Syntax
{
    internal readonly struct SyntaxTokenInfo
    {
        private readonly SyntaxToken token;
        private readonly SyntaxTokenPrecedence precedence;
        private readonly ExpressionType expressionType;

        public SyntaxTokenInfo(in SyntaxToken token, SyntaxTokenPrecedence precedence, ExpressionType expressionType)
        {
            this.token = token;
            this.precedence = precedence;
            this.expressionType = expressionType;
        }

        public SyntaxToken Token => token;

        public SyntaxTokenPrecedence Precedence => precedence;

        public ExpressionType ExpressionType => expressionType;
    }
}
