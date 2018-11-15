using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class SinLambdaExpression : SystemLambdaExpression
    {
        public SinLambdaExpression()
            : base(typeof(Math).GetMethod("Sin", new Type[] { typeof(double) }), "sin", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Sin(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}
