using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class ExpLambdaExpression : SystemLambdaExpression
    {
        public ExpLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Exp), new Type[] { typeof(double) })!, "exp", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Exp((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}
