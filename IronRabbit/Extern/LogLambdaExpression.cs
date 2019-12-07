using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class LogLambdaExpression : SystemLambdaExpression
    {
        public LogLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Log), new Type[] { typeof(double), typeof(double) }), "log", new BodyExpression(), 
                  Expression.Parameter(typeof(double), "x"), 
                  Expression.Parameter(typeof(double), "e"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                var x = ParameterExpression.Access<double>(context, "x");
                var e = ParameterExpression.Access<double>(context, "e");
                return Math.Log(x, e);
            }
        }
    }
}
