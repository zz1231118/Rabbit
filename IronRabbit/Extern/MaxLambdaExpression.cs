using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class MaxLambdaExpression : SystemLambdaExpression
    {
        public MaxLambdaExpression()
            : base(typeof(Math).GetMethod("Max", new Type[] { typeof(double), typeof(double) }), "max", new BodyExpression(), Expression.Parameter(typeof(double), "lh"), Expression.Parameter(typeof(double), "rh"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                double lh = ParameterExpression.Access<double>(context, "lh");
                double rh = ParameterExpression.Access<double>(context, "rh");
                return Math.Max(lh, rh);
            }
        }
    }
}
