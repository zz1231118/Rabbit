using IronRabbit.Syntax;

namespace IronRabbit.Compiler
{
    internal abstract class SyntaxParser
    {
        private readonly Lexer lexer;
        private SyntaxToken current;

        protected SyntaxParser(Lexer lexer)
        {
            if (lexer == null) throw new ArgumentNullException(nameof(lexer));

            this.lexer = lexer;
            this.current = lexer.Lex();
        }

        protected SyntaxToken Current => current;

        protected SyntaxToken EatToken()
        {
            var currentToken = current;
            current = lexer.Lex();
            return currentToken;
        }

        protected SyntaxToken EatToken(SyntaxTokenKind kind)
        {
            var currentToken = current;
            if (currentToken.Kind == kind)
            {
                current = lexer.Lex();
                return currentToken;
            }

            throw new CompilerException(currentToken);
        }

        protected SyntaxToken EatToken(SyntaxTokenKind kind, string value)
        {
            var currentToken = current;
            if (currentToken.Kind == kind && currentToken.Text == value)
            {
                current = lexer.Lex();
                return currentToken;
            }

            throw new CompilerException(currentToken);
        }

        protected bool TryEatToken(SyntaxTokenKind kind)
        {
            var currentToken = current;
            if (currentToken.Kind == kind)
            {
                current = lexer.Lex();
                return true;
            }

            return false;
        }

        protected bool TryEatToken(SyntaxTokenKind kind, string value)
        {
            var currentToken = current;
            if (currentToken.Kind == kind && currentToken.Text == value)
            {
                current = lexer.Lex();
                return true;
            }

            return false;
        }

        protected CompilerException CreateException()
        {
            return new CompilerException(current);
        }

        protected CompilerException CreateException(SyntaxToken token)
        {
            return new CompilerException(token);
        }
    }
}
