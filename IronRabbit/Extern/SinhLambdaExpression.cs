using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class SinhLambdaExpression : SystemLambdaExpression
    {
        public SinhLambdaExpression()
            : base(typeof(Math).GetMethod("Sinh", new Type[] { typeof(double) }), "sinh", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Sinh(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}
