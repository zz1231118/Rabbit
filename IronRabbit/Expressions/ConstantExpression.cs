using System;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class ConstantExpression : Expression
    {
        internal ConstantExpression(decimal value)
            : base(ExpressionType.Constant)
        {
            Value = value;
        }

        public decimal Value { get; }

        public override decimal Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return Value;
        }
    }
}
