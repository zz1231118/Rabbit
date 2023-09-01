using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class MinLambdaExpression : SystemLambdaExpression
    {
        public MinLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Min), new Type[] { typeof(decimal), typeof(decimal) })!, "min", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "left"),
                  Expression.Parameter(typeof(decimal), "right"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                var left = ParameterExpression.Access<decimal>(context, "left");
                var right = ParameterExpression.Access<decimal>(context, "right");
                return Math.Min(left, right);
            }
        }
    }
}
