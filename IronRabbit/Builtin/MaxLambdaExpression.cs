using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class MaxLambdaExpression : SystemLambdaExpression
    {
        public MaxLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Max), new Type[] { typeof(decimal), typeof(decimal) })!, "max", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "left"),
                  Expression.Parameter(typeof(decimal), "right"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                var left = ParameterExpression.Access<decimal>(context, "left");
                var right = ParameterExpression.Access<decimal>(context, "right");
                return Math.Max(left, right);
            }
        }
    }
}
