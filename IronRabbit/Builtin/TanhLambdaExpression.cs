using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class TanhLambdaExpression : SystemLambdaExpression
    {
        public TanhLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Tanh), new Type[] { typeof(double) })!, "tanh", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Tanh((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}
