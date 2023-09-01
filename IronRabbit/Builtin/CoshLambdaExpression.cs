using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class CoshLambdaExpression : SystemLambdaExpression
    {
        public CoshLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Cosh), new Type[] { typeof(double) })!, "cosh", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Cosh((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}
