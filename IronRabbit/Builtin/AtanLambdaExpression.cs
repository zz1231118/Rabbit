using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class AtanLambdaExpression : SystemLambdaExpression
    {
        public AtanLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Atan), new Type[] { typeof(double) })!, "atan", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Atan((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}