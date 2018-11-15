using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class CosLambdaExpression : SystemLambdaExpression
    {
        public CosLambdaExpression()
            : base(typeof(Math).GetMethod("Cos", new Type[] { typeof(double) }), "cos", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Cos(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}
