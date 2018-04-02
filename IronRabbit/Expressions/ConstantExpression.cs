using System;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class ConstantExpression : Expression
    {
        internal ConstantExpression(double value)
            : base(ExpressionType.Constant)
        {
            Value = value;
        }

        public double Value { get; }

        public override double Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return Value;
        }
    }
}
