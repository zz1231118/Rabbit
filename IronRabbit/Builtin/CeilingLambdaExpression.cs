using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class CeilingLambdaExpression : SystemLambdaExpression
    {
        public CeilingLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Ceiling), new Type[] { typeof(decimal) })!, "ceiling", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Ceiling(ParameterExpression.Access<decimal>(context, "x"));
            }
        }
    }
}
