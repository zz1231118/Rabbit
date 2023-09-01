using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class FloorLambdaExpression : SystemLambdaExpression
    {
        public FloorLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Floor), new Type[] { typeof(decimal) })!, "floor", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Floor(ParameterExpression.Access<decimal>(context, "x"));
            }
        }
    }
}