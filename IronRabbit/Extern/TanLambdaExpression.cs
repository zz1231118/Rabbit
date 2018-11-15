using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class TanLambdaExpression : SystemLambdaExpression
    {
        public TanLambdaExpression()
            : base(typeof(Math).GetMethod("Tan", new Type[] { typeof(double) }), "tan", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Tan(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}