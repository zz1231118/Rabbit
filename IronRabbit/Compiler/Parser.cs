using System;
using System.Collections.Generic;
using IronRabbit.Expressions;

namespace IronRabbit.Compiler
{
    internal class Parser
    {
        private readonly Tokenizer _tokenizer;
        private TokenWithSpan _lookahead;
        private TokenWithSpan _token;

        public Parser(Tokenizer tokenizer)
        {
            if (tokenizer == null)
                throw new ArgumentNullException(nameof(tokenizer));

            _tokenizer = tokenizer;
            Initialize();
        }

        private void Initialize()
        {
            FetchLookahead();
        }
        private Token NextToken()
        {
            _token = _lookahead;
            FetchLookahead();
            return _token.Token;
        }
        private Token PeekToken()
        {
            return this._lookahead.Token;
        }
        private void FetchLookahead()
        {
            _lookahead = new TokenWithSpan(_tokenizer.NextToken(), _tokenizer.TokenSpan);
        }
        private bool PeekToken(TokenKind kind)
        {
            return PeekToken().Kind == kind;
        }
        private bool PeekToken(Token check)
        {
            return PeekToken() == check;
        }
        private bool MaybeEat(TokenKind kind)
        {
            if (PeekToken().Kind == kind)
            {
                NextToken();
                return true;
            }
            return false;
        }

        public LambdaExpression Parse()
        {
            return ParseStatement();
        }

        private LambdaExpression ParseStatement()
        {
            switch (PeekToken().Kind)
            {
                case TokenKind.Comment:
                    NextToken();
                    return ParseStatement();
                default:
                    return ParseLambdaExpression();
            }
        }
        private ExpressionType GetBinaryOperator(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Add:
                    return ExpressionType.Add;
                case TokenKind.Subtract:
                    return ExpressionType.Subtract;
                case TokenKind.Multiply:
                    return ExpressionType.Multiply;
                case TokenKind.Divide:
                    return ExpressionType.Divide;
                case TokenKind.Mod:
                    return ExpressionType.Modulo;
                case TokenKind.Power:
                    return ExpressionType.Power;
                default:
                    throw new CompilerException(_tokenizer.Position, string.Format("operator TokenKind:{0} error!", kind.ToString()));
            }
        }

        private IList<Expression> ParseArguments()
        {
            List<Expression> list = new List<Expression>();
            Token token = PeekToken();
            if (token.Kind != TokenKind.RightParen)
            {
                list.Add(ParseExpression());
                token = PeekToken();
                while (MaybeEat(TokenKind.Comma))
                {
                    list.Add(ParseExpression());
                }
            }

            return list;
        }
        private Expression ParseTerm()
        {
            Expression expr = null;
            Token token = NextToken();
            switch (token.Kind)
            {
                case TokenKind.Constant:
                    expr = Expression.Constant(token.Value);
                    break;
                case TokenKind.Subtract:
                    expr = Expression.Negate(ParseExpression());
                    break;
                case TokenKind.LeftParen:
                    expr = ParseExpression();
                    if (!MaybeEat(TokenKind.RightParen))
                        throw new CompilerException(_tokenizer.Position, PeekToken().Text);
                    break;
                case TokenKind.Identifier:
                    switch (PeekToken().Kind)
                    {
                        case TokenKind.LeftParen:
                            NextToken();
                            expr = Expression.Call(null, token.Text, ParseArguments());
                            if (!MaybeEat(TokenKind.RightParen))
                                throw new CompilerException(_tokenizer.Position, PeekToken().Text);

                            break;
                        case TokenKind.Dot:
                            expr = Expression.Parameter(token.Text);
                            while (MaybeEat(TokenKind.Dot))
                            {
                                token = NextToken();
                                if (token.Kind != TokenKind.Identifier)
                                    throw new CompilerException(_tokenizer.Position, PeekToken().Text);
                                if (PeekToken().Kind == TokenKind.LeftParen)
                                {
                                    expr = Expression.Call(expr, token.Text, ParseArguments());
                                    if (!MaybeEat(TokenKind.RightParen))
                                        throw new CompilerException(_tokenizer.Position, PeekToken().Text);
                                    break;
                                }

                                expr = Expression.Member(expr, token.Text);
                            }
                            if (MaybeEat(TokenKind.LeftParen))
                            {
                                expr = Expression.Call(expr, token.Text, ParseArguments());
                                if (!MaybeEat(TokenKind.RightParen))
                                    throw new CompilerException(_tokenizer.Position, PeekToken().Text);
                            }
                            break;
                        default:
                            expr = Expression.Parameter(token.Text);
                            break;
                    }
                    break;
            }

            return expr;
        }
        private Expression ParseExpression(byte precedence = 0)
        {
            Expression leftOperand = ParseTerm();
            while (true)
            {
                Token token = PeekToken();
                if (!(token is Token.OperatorToken operToken))
                {
                    break;
                }
                if (operToken.Precedence < precedence)
                {
                    break;
                }

                NextToken();
                Expression rightOperand = ParseExpression(checked((byte)(operToken.Precedence + 1)));
                ExpressionType @operator = GetBinaryOperator(token.Kind);
                leftOperand = new BinaryExpression(@operator, leftOperand, rightOperand);
            }

            return leftOperand;
        }
        private LambdaExpression ParseLambdaExpression()
        {
            Token token = NextToken();
            if (token.Kind != TokenKind.Identifier)
                throw new CompilerException(_tokenizer.Position, token.Text);

            string name = token.Text;
            if (!MaybeEat(TokenKind.LeftParen))
                throw new CompilerException(_tokenizer.Position, PeekToken().Text);

            var parameters = new List<ParameterExpression>();
            if (!MaybeEat(TokenKind.RightParen))
            {
                token = NextToken();
                if (token.Kind != TokenKind.Identifier)
                    throw new CompilerException(_tokenizer.Position, token.Text);

                parameters.Add(Expression.Parameter(token.Text));
                token = NextToken();
                while (token.Kind == TokenKind.Comma)
                {
                    token = NextToken();
                    if (token.Kind != TokenKind.Identifier)
                        throw new CompilerException(_tokenizer.Position, token.Text);

                    parameters.Add(Expression.Parameter(token.Text));
                    token = NextToken();
                }

                if (token.Kind != TokenKind.RightParen)
                    throw new CompilerException(_tokenizer.Position, token.Text);
            }
            if (!MaybeEat(TokenKind.Assign))
                throw new CompilerException(_tokenizer.Position, PeekToken().Text);

            Expression body = ParseExpression();
            return Expression.Lambda(name, body, parameters);
        }
    }
}
