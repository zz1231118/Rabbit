using System;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class UnaryExpression : Expression
    {
        internal UnaryExpression(ExpressionType type, Expression operand)
            : base(type)
        {
            Operand = operand;
        }

        public Expression Operand { get; }

        public override object Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            object value = Operand.Eval(context);
            switch (NodeType)
            {
                case ExpressionType.Negate:
                    return (-(double)value);
                case ExpressionType.Not:
                    return (!(bool)value);
                default:
                    throw new RuntimeException("unknown unary:" + NodeType.ToString());
            }
        }
        public override string ToString()
        {
            switch (NodeType)
            {
                case ExpressionType.Negate:
                    return Operand is BinaryExpression ? "-(" + Operand.ToString() + ")" : "-" + Operand.ToString();
                case ExpressionType.Not:
                    return Operand is BinaryExpression ? "!(" + Operand.ToString() + ")" : "!" + Operand.ToString();
                default:
                    throw new RuntimeException("unknown operator:" + NodeType.ToString());
            }
                    
        }
    }
}