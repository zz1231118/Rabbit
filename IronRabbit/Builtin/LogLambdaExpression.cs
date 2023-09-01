using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class LogLambdaExpression : SystemLambdaExpression
    {
        public LogLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Log), new Type[] { typeof(double), typeof(double) })!, "log", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"),
                  Expression.Parameter(typeof(decimal), "e"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                var x = ParameterExpression.Access<decimal>(context, "x");
                var e = ParameterExpression.Access<decimal>(context, "e");
                return checked((decimal)Math.Log((double)x, (double)e));
            }
        }
    }
}
