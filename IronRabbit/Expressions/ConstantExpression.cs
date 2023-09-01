using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class ConstantExpression : Expression
    {
        internal ConstantExpression(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public override ExpressionType NodeType => ExpressionType.Constant;

        public override Type Type => Value is null ? typeof(object) : Value.GetType();

        public override object Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return Value;
        }

        public override string? ToString()
        {
            return Value.ToString();
        }
    }
}
