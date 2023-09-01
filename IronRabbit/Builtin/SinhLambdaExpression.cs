using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class SinhLambdaExpression : SystemLambdaExpression
    {
        public SinhLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Sinh), new Type[] { typeof(double) })!, "sinh", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Sinh((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}
