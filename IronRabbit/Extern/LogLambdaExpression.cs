using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class LogLambdaExpression : SystemLambdaExpression
    {
        public LogLambdaExpression()
            : base(typeof(Math).GetMethod("Log", new Type[] { typeof(double), typeof(double) }), "log", new BodyExpression(), Expression.Parameter(typeof(double), "x"), Expression.Parameter(typeof(double), "e"))
        { }

        class BodyExpression : Expression
        {
            public override double Eval(RuntimeContext context)
            {
                double x = ParameterExpression.Access(context, "x");
                double e = ParameterExpression.Access(context, "e");
                return Math.Log(x, e);
            }
        }
    }
}
