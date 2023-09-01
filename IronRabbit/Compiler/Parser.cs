using IronRabbit.Expressions;
using IronRabbit.Syntax;

namespace IronRabbit.Compiler
{
    internal sealed class Parser : SyntaxParser
    {
        public Parser(Lexer lexer)
            : base(lexer)
        { }

        private static SyntaxTokenInfo GetSyntaxTokenInfo(SyntaxToken token)
        {
            var precedence = token.Kind switch
            {
                SyntaxTokenKind.Equal or 
                SyntaxTokenKind.NotEqual => SyntaxTokenPrecedence.Equality,
                SyntaxTokenKind.LessThan or
                SyntaxTokenKind.LessThanOrEqual or
                SyntaxTokenKind.GreaterThan or
                SyntaxTokenKind.GreaterThanOrEqual => SyntaxTokenPrecedence.Relational,
                SyntaxTokenKind.Add or
                SyntaxTokenKind.Subtract => SyntaxTokenPrecedence.Additive,
                SyntaxTokenKind.Multiply or
                SyntaxTokenKind.Divide or
                SyntaxTokenKind.Mod or
                SyntaxTokenKind.Power => SyntaxTokenPrecedence.Mutiplicative,
                _ => SyntaxTokenPrecedence.Expression,
            };
            var expressionType = token.Kind switch
            {
                SyntaxTokenKind.Add => ExpressionType.Add,
                SyntaxTokenKind.Subtract => ExpressionType.Subtract,
                SyntaxTokenKind.Multiply => ExpressionType.Multiply,
                SyntaxTokenKind.Divide => ExpressionType.Divide,
                SyntaxTokenKind.Mod => ExpressionType.Modulo,
                SyntaxTokenKind.Power => ExpressionType.Power,
                SyntaxTokenKind.LessThan => ExpressionType.LessThan,
                SyntaxTokenKind.LessThanOrEqual => ExpressionType.LessThanOrEqual,
                SyntaxTokenKind.Equal => ExpressionType.Equal,
                SyntaxTokenKind.GreaterThanOrEqual => ExpressionType.GreaterThanOrEqual,
                SyntaxTokenKind.GreaterThan => ExpressionType.GreaterThan,
                SyntaxTokenKind.NotEqual => ExpressionType.NotEqual,
                _ => ExpressionType.None,
            };
            return new SyntaxTokenInfo(token, precedence, expressionType);
        }

        private IList<Expression> ParseArguments()
        {
            List<Expression> list = new List<Expression>();
            if (Current.Kind != SyntaxTokenKind.RightParen)
            {
                do
                {
                    list.Add(ParseExpression());
                } while (TryEatToken(SyntaxTokenKind.Comma));
            }

            return list;
        }

        private Expression ParseTerm()
        {
            Expression expr;
            SyntaxToken token = EatToken();
            switch (token.Kind)
            {
                case SyntaxTokenKind.Numeric:
                    expr = Expression.Constant((decimal)token.Value);
                    break;
                case SyntaxTokenKind.Subtract:
                    expr = Expression.Negate(ParseTerm());
                    break;
                case SyntaxTokenKind.LeftParen:
                    expr = ParseExpression();
                    EatToken(SyntaxTokenKind.RightParen);
                    break;
                case SyntaxTokenKind.Symbol:
                    switch (token.Text)
                    {
                        case "if":
                            {
                                EatToken(SyntaxTokenKind.LeftParen);
                                var test = ParseExpression();
                                EatToken(SyntaxTokenKind.Comma);
                                var trueExpre = ParseExpression();
                                EatToken(SyntaxTokenKind.Comma);
                                var falseExpre = ParseExpression();
                                EatToken(SyntaxTokenKind.RightParen);
                                return new ConditionalExpression(test, trueExpre, falseExpre);
                            }
                    }
                    switch (Current.Kind)
                    {
                        case SyntaxTokenKind.LeftParen:
                            EatToken();
                            expr = Expression.Call(null, token.Text, ParseArguments());
                            EatToken(SyntaxTokenKind.RightParen);
                            break;
                        case SyntaxTokenKind.Dot:
                            EatToken();
                            expr = Expression.Parameter(typeof(object), token.Text);
                            expr = Expression.Member(expr, EatToken(SyntaxTokenKind.Symbol).Text);
                            break;
                        default:
                            expr = Expression.Parameter(typeof(decimal), token.Text);
                            break;
                    }
                    break;
                case SyntaxTokenKind.Not:
                    return Expression.Not(ParseTerm());
                default:
                    throw new CompilerException(Current);
            }

            return expr;
        }

        private Expression ParseExpression(SyntaxTokenPrecedence precedence = SyntaxTokenPrecedence.Expression)
        {
            var leftOperand = ParseTerm();
            while (true)
            {
                var info = GetSyntaxTokenInfo(Current);
                if (info.Precedence <= SyntaxTokenPrecedence.Expression || info.Precedence < precedence)
                {
                    break;
                }

                EatToken();
                var rightOperand = ParseExpression(info.Precedence);
                var type = info.Precedence == SyntaxTokenPrecedence.Equality || info.Precedence == SyntaxTokenPrecedence.Relational ? typeof(bool) : leftOperand.Type;
                leftOperand = new BinaryExpression(info.ExpressionType, leftOperand, rightOperand, type);
            }

            return leftOperand;
        }

        private LambdaExpression ParseLambdaExpression()
        {
            var nameToken = EatToken(SyntaxTokenKind.Symbol);
            var parameters = new List<ParameterExpression>();
            if (TryEatToken(SyntaxTokenKind.LeftParen))
            {
                if (!TryEatToken(SyntaxTokenKind.RightParen))
                {
                    do
                    {
                        var token = EatToken(SyntaxTokenKind.Symbol);
                        parameters.Add(Expression.Parameter(typeof(decimal), token.Text));
                    } while (TryEatToken(SyntaxTokenKind.Comma));

                    EatToken(SyntaxTokenKind.RightParen);
                }
            }

            EatToken(SyntaxTokenKind.Assign);
            var body = ParseExpression();
            return Expression.Lambda(nameToken.Text, body, parameters);
        }

        public LambdaExpression? Parse()
        {
            while (true)
            {
                switch (Current.Kind)
                {
                    case SyntaxTokenKind.Annotation:
                    case SyntaxTokenKind.NewLine:
                        EatToken();
                        continue;
                    case SyntaxTokenKind.EndOfFile:
                        return null;
                    default:
                        return ParseLambdaExpression();
                }
            }
        }
    }
}
