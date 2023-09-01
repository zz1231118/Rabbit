using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class AsinLambdaExpression : SystemLambdaExpression
    {
        public AsinLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Asin), new Type[] { typeof(double) })!, "asin", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Asin((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}