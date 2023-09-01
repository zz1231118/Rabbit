using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class SqrtLambdaExpression : SystemLambdaExpression
    {
        public SqrtLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Sqrt), new Type[] { typeof(double) })!, "sqrt", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Sqrt((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}
