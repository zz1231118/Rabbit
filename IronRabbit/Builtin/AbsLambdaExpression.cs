using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class AbsLambdaExpression : SystemLambdaExpression
    {
        public AbsLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Abs), new Type[] { typeof(decimal) })!, "abs", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Abs(ParameterExpression.Access<decimal>(context, "x"));
            }
        }
    }
}