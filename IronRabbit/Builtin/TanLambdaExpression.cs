using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class TanLambdaExpression : SystemLambdaExpression
    {
        public TanLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Tan), new Type[] { typeof(double) })!, "tan", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Tan((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}