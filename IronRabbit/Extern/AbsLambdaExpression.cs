using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class AbsLambdaExpression : SystemLambdaExpression
    {
        public AbsLambdaExpression()
            : base(typeof(Math).GetMethod("Abs", new Type[] { typeof(double) }), "abs", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override double Eval(RuntimeContext context)
            {
                return Math.Abs(ParameterExpression.Access(context, "x"));
            }
        }
    }
}