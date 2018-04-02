using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class ExpLambdaExpression : SystemLambdaExpression
    {
        public ExpLambdaExpression()
            : base(typeof(Math).GetMethod("Exp", new Type[] { typeof(double) }), "exp", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override double Eval(RuntimeContext context)
            {
                return Math.Exp(ParameterExpression.Access(context, "x"));
            }
        }
    }
}
