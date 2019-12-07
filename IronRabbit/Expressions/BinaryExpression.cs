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

            object lh = Left.Eval(context);
            object rh = Right.Eval(context);
            switch (NodeType)
            {
                case ExpressionType.Add:
                    return System.Convert.ToDouble(lh) + System.Convert.ToDouble(rh);
                case ExpressionType.Subtract:
                    return System.Convert.ToDouble(lh) - System.Convert.ToDouble(rh);
                case ExpressionType.Multiply:
                    return System.Convert.ToDouble(lh) * System.Convert.ToDouble(rh);
                case ExpressionType.Divide:
                    return System.Convert.ToDouble(lh) / System.Convert.ToDouble(rh);
                case ExpressionType.Modulo:
                    return System.Convert.ToDouble(lh) % System.Convert.ToDouble(rh);
                case ExpressionType.Power:
                    return Math.Pow(System.Convert.ToDouble(lh), System.Convert.ToDouble(rh));
                case ExpressionType.LessThan:
                    return System.Convert.ToDouble(lh) < System.Convert.ToDouble(rh);
                case ExpressionType.LessThanOrEqual:
                    return System.Convert.ToDouble(lh) <= System.Convert.ToDouble(rh);
                case ExpressionType.Equal:
                    return lh is bool ? System.Convert.ToBoolean(lh) == System.Convert.ToBoolean(rh) : System.Convert.ToDouble(lh) == System.Convert.ToDouble(rh);
                case ExpressionType.GreaterThanOrEqual:
                    return System.Convert.ToDouble(lh) >= System.Convert.ToDouble(rh);
                case ExpressionType.GreaterThan:
                    return System.Convert.ToDouble(lh) > System.Convert.ToDouble(rh);
                case ExpressionType.NotEqual:
                    return lh is bool ? System.Convert.ToBoolean(lh) != System.Convert.ToBoolean(rh) : System.Convert.ToDouble(lh) != System.Convert.ToDouble(rh);
                default:
                    throw new RuntimeException("unknown operator:" + NodeType.ToString());
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Left is BinaryExpression)
                sb.Append('(');

            sb.Append(Left.ToString());
            if (Left is BinaryExpression)
                sb.Append(')');

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
