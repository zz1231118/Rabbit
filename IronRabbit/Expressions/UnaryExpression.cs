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

        public override double Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            double value = Operand.Eval(context);
            switch (NodeType)
            {
                case ExpressionType.Negate:
                    return -value;
                default:
                    throw new RuntimeException("unknown unary:" + NodeType.ToString());
            }
        }
    }
}