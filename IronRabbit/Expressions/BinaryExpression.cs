using System;
using System.Text;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class BinaryExpression : Expression
    {
        internal BinaryExpression(ExpressionType nodeType, Expression left, Expression right)
            : base(nodeType)
        {
            Left = left;
            Right = right;
        }

        public Expression Left { get; }

        public Expression Right { get; }

        public override object Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var leftResult = Left.Eval(context);
            var rightResult = Right.Eval(context);
            return NodeType switch
            {
                ExpressionType.Add => System.Convert.ToDouble(leftResult) + System.Convert.ToDouble(rightResult),
                ExpressionType.Subtract => System.Convert.ToDouble(leftResult) - System.Convert.ToDouble(rightResult),
                ExpressionType.Multiply => System.Convert.ToDouble(leftResult) * System.Convert.ToDouble(rightResult),
                ExpressionType.Divide => System.Convert.ToDouble(leftResult) / System.Convert.ToDouble(rightResult),
                ExpressionType.Modulo => System.Convert.ToDouble(leftResult) % System.Convert.ToDouble(rightResult),
                ExpressionType.Power => Math.Pow(System.Convert.ToDouble(leftResult), System.Convert.ToDouble(rightResult)),
                ExpressionType.LessThan => System.Convert.ToDouble(leftResult) < System.Convert.ToDouble(rightResult),
                ExpressionType.LessThanOrEqual => System.Convert.ToDouble(leftResult) <= System.Convert.ToDouble(rightResult),
                ExpressionType.Equal => leftResult is bool ? System.Convert.ToBoolean(leftResult) == System.Convert.ToBoolean(rightResult) : System.Convert.ToDouble(leftResult) == System.Convert.ToDouble(rightResult),
                ExpressionType.GreaterThanOrEqual => System.Convert.ToDouble(leftResult) >= System.Convert.ToDouble(rightResult),
                ExpressionType.GreaterThan => System.Convert.ToDouble(leftResult) > System.Convert.ToDouble(rightResult),
                ExpressionType.NotEqual => leftResult is bool ? System.Convert.ToBoolean(leftResult) != System.Convert.ToBoolean(rightResult) : System.Convert.ToDouble(leftResult) != System.Convert.ToDouble(rightResult),
                _ => throw new RuntimeException("unknown operator:" + NodeType.ToString()),
            };
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Left is BinaryExpression)
            {
                sb.Append('(');
                sb.Append(Left.ToString());
                sb.Append(')');
            }
            else
            {
                sb.Append(Left.ToString());
            }
            switch (NodeType)
            {
                case ExpressionType.Add:
                    sb.Append('+');
                    break;
                case ExpressionType.Subtract:
                    sb.Append('-');
                    break;
                case ExpressionType.Multiply:
                    sb.Append('*');
                    break;
                case ExpressionType.Divide:
                    sb.Append('/');
                    break;
                case ExpressionType.Modulo:
                    sb.Append('%');
                    break;
                case ExpressionType.Power:
                    sb.Append('^');
                    break;

                case ExpressionType.LessThan:
                    sb.Append('<');
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append("<=");
                    break;
                case ExpressionType.Equal:
                    sb.Append("==");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(">=");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append('>');
                    break;
                case ExpressionType.NotEqual:
                    sb.Append("!=");
                    break;
                default:
                    throw new RuntimeException("unknown operator:" + NodeType.ToString());
            }
            if (Right is BinaryExpression)
                sb.Append('(');

            sb.Append(Right.ToString());
            if (Right is BinaryExpression)
                sb.Append(')');

            return sb.ToString();
        }
    }
}
