using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class MinLambdaExpression : SystemLambdaExpression
    {
        public MinLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Min), new Type[] { typeof(double), typeof(double) }), "min", new BodyExpression(), 
                  Expression.Parameter(typeof(double), "left"), 
                  Expression.Parameter(typeof(double), "right"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                var left = ParameterExpression.Access<double>(context, "left");
                var right = ParameterExpression.Access<double>(context, "right");
                return Math.Min(left, right);
            }
        }
    }
}
