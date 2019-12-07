using System;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class ConstantExpression : Expression
    {
        internal ConstantExpression(object value)
            : base(ExpressionType.Constant)
        {
            Value = value;
        }

        public object Value { get; }

        public override object Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
