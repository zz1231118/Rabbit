using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class CosLambdaExpression : SystemLambdaExpression
    {
        public CosLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Cos), new Type[] { typeof(double) })!, "cos", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Cos((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}
