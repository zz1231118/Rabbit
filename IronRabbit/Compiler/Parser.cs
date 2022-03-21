using System;
using System.Collections.Generic;
using IronRabbit.Expressions;
using IronRabbit.Syntax;

namespace IronRabbit.Compiler
{
    internal class Parser
    {
        private readonly Tokenizer tokenizer;
        private TokenWithSpan lookahead;
        private TokenWithSpan token;

        public Parser(Tokenizer tokenizer)
        {
            if (tokenizer == null)
                throw new ArgumentNullException(nameof(tokenizer));

            this.tokenizer = tokenizer;
            Initialize();
        }

        private void Initialize()
        {
            FetchLookahead();
        }

        private Token NextToken()
        {
            token = lookahead;
            FetchLookahead();
            return token.Token;
        }

        private Token PeekToken()
        {
            return this.lookahead.Token;
        }

        private void FetchLookahead()
        {
            lookahead = new TokenWithSpan(tokenizer.NextToken(), tokenizer.TokenSpan);
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

        private LambdaExpression ParseStatement()
        {
            while (true)
            {
                switch (PeekToken().Kind)
                {
                    case TokenKind.Comment:
                    case TokenKind.NewLine:
                        NextToken();
                        continue;
                    case TokenKind.EndOfFile:
                        return null;
                    default:
                        return ParseLambdaExpression();
                }
            }
        }

        private ExpressionType GetBinaryOperator(TokenKind kind)
        {
            return kind switch
            {
                TokenKind.Add => ExpressionType.Add,
                TokenKind.Subtract => ExpressionType.Subtract,
                TokenKind.Multiply => ExpressionType.Multiply,
                TokenKind.Divide => ExpressionType.Divide,
                TokenKind.Mod => ExpressionType.Modulo,
                TokenKind.Power => ExpressionType.Power,
                TokenKind.LessThan => ExpressionType.LessThan,
                TokenKind.LessThanOrEqual => ExpressionType.LessThanOrEqual,
                TokenKind.Equal => ExpressionType.Equal,
                TokenKind.GreaterThanOrEqual => ExpressionType.GreaterThanOrEqual,
                TokenKind.GreaterThan => ExpressionType.GreaterThan,
                TokenKind.NotEqual => ExpressionType.NotEqual,
                _ => throw new CompilerException(tokenizer.Position, string.Format("operator TokenKind:{0} error!", kind.ToString())),
            };
        }

        private IList<Expression> ParseArguments()
        {
            List<Expression> list = new List<Expression>();
            if (PeekToken().Kind != TokenKind.RightParen)
            {
                do
                {
                    list.Add(ParseExpression());
                } while (MaybeEat(TokenKind.Comma));
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
                        throw new CompilerException(tokenizer.Position, PeekToken().Text);
                    break;
                case TokenKind.Identifier:
                    switch (PeekToken().Kind)
                    {
                        case TokenKind.LeftParen:
                            NextToken();
                            expr = Expression.Call(null, token.Text, ParseArguments());
                            if (!MaybeEat(TokenKind.RightParen))
                                throw new CompilerException(tokenizer.Position, PeekToken().Text);

                            break;
                        case TokenKind.Dot:
                            expr = Expression.Parameter(null, token.Text);
                            while (MaybeEat(TokenKind.Dot))
                            {
                                token = NextToken();
                                if (token.Kind != TokenKind.Identifier)
                                    throw new CompilerException(tokenizer.Position, PeekToken().Text);
                                if (PeekToken().Kind == TokenKind.LeftParen)
                                {
                                    expr = Expression.Call(expr, token.Text, ParseArguments());
                                    if (!MaybeEat(TokenKind.RightParen))
                                        throw new CompilerException(tokenizer.Position, PeekToken().Text);
                                    break;
                                }

                                expr = Expression.Member(expr, token.Text);
                            }
                            if (MaybeEat(TokenKind.LeftParen))
                            {
                                expr = Expression.Call(expr, token.Text, ParseArguments());
                                if (!MaybeEat(TokenKind.RightParen))
                                    throw new CompilerException(tokenizer.Position, PeekToken().Text);
                            }
                            break;
                        default:
                            expr = Expression.Parameter(typeof(double), token.Text);
                            break;
                    }
                    break;
                case TokenKind.Not:
                    return Expression.Not(ParseExpression()); 
                case TokenKind.IF:
                    if (!MaybeEat(TokenKind.LeftParen))
                        throw new CompilerException(tokenizer.Position, PeekToken().Text);
                    var test = ParseExpression();
                    if (!MaybeEat(TokenKind.Comma))
                        throw new CompilerException(tokenizer.Position, PeekToken().Text);
                    var trueExpre = ParseExpression();
                    if (!MaybeEat(TokenKind.Comma))
                        throw new CompilerException(tokenizer.Position, PeekToken().Text);
                    var falseExpre = ParseExpression();
                    if (!MaybeEat(TokenKind.RightParen))
                        throw new CompilerException(tokenizer.Position, PeekToken().Text);

                    expr = Expression.Condition(test, trueExpre, falseExpre);
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
                if (token is OperatorToken operatorToken && operatorToken.Precedence >= precedence)
                {
                    NextToken();
                    Expression rightOperand = ParseExpression(checked((byte)(operatorToken.Precedence + 1)));
                    ExpressionType @operator = GetBinaryOperator(token.Kind);
                    leftOperand = new BinaryExpression(@operator, leftOperand, rightOperand);
                    continue;
                }

                break;
            }

            return leftOperand;
        }

        private LambdaExpression ParseLambdaExpression()
        {
            Token token = NextToken();
            if (token.Kind != TokenKind.Identifier)
                throw new CompilerException(tokenizer.Position, token.Text);

            var name = token.Text;
            var parameters = new List<ParameterExpression>();
            if (MaybeEat(TokenKind.LeftParen))
            {
                if (!MaybeEat(TokenKind.RightParen))
                {
                    do
                    {
                        token = NextToken();
                        if (token.Kind != TokenKind.Identifier)
                            throw new CompilerException(tokenizer.Position, token.Text);

                        parameters.Add(Expression.Parameter(typeof(double), token.Text));
                    } while (MaybeEat(TokenKind.Comma));

                    if (!MaybeEat(TokenKind.RightParen))
                        throw new CompilerException(tokenizer.Position, token.Text);
                }
            }
            if (!MaybeEat(TokenKind.Assign))
                throw new CompilerException(tokenizer.Position, PeekToken().Text);

            Expression body = ParseExpression();
            return Expression.Lambda(name, body, parameters);
        }

        public LambdaExpression Parse()
        {
            return ParseStatement();
        }
    }
}
