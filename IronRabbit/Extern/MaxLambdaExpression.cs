using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class MaxLambdaExpression : SystemLambdaExpression
    {
        public MaxLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Max), new Type[] { typeof(double), typeof(double) }), "max", new BodyExpression(), 
                  Expression.Parameter(typeof(double), "left"), 
                  Expression.Parameter(typeof(double), "right"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                var left = ParameterExpression.Access<double>(context, "left");
                var right = ParameterExpression.Access<double>(context, "right");
                return Math.Max(left, right);
            }
        }
    }
}
