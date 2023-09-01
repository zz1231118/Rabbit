using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class RoundLambdaExpression : SystemLambdaExpression
    {
        public RoundLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Round), new Type[] { typeof(decimal), typeof(int) })!, "round", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "value"),
                  Expression.Parameter(typeof(decimal), "digits"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                var value = ParameterExpression.Access<decimal>(context, "value");
                var digits = (int)ParameterExpression.Access<decimal>(context, "digits");
                return Math.Round(value, digits);
            }
        }
    }
}
