using System;
using System.Linq.Expressions;
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

        public override double Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            double lh = Left.Eval(context);
            double rh = Right.Eval(context);
            switch (NodeType)
            {
                case ExpressionType.Add:
                    return lh + rh;
                case ExpressionType.Subtract:
                    return lh - rh;
                case ExpressionType.Multiply:
                    return lh * rh;
                case ExpressionType.Divide:
                    return lh / rh;
                case ExpressionType.Modulo:
                    return lh % rh;
                case ExpressionType.Power:
                    return Math.Pow(lh, rh);
                default:
                    throw new RuntimeException("unknown operator char:" + NodeType.ToString());
            }
        }
    }
}
