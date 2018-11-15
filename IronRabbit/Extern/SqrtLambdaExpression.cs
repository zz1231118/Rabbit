using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class SqrtLambdaExpression : SystemLambdaExpression
    {
        public SqrtLambdaExpression()
            : base(typeof(Math).GetMethod("Sqrt", new Type[] { typeof(double) }), "sqrt", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Sqrt(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}
