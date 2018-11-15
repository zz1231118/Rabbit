using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class MinLambdaExpression : SystemLambdaExpression
    {
        public MinLambdaExpression()
            : base(typeof(Math).GetMethod("Min", new Type[] { typeof(double), typeof(double) }), "min", new BodyExpression(), Expression.Parameter(typeof(double), "lh"), Expression.Parameter(typeof(double), "rh"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                double lh = ParameterExpression.Access<double>(context, "lh");
                double rh = ParameterExpression.Access<double>(context, "rh");
                return Math.Min(lh, rh);
            }
        }
    }
}
