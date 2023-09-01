using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class SinLambdaExpression : SystemLambdaExpression
    {
        public SinLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Sin), new Type[] { typeof(double) })!, "sin", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Sin((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}
