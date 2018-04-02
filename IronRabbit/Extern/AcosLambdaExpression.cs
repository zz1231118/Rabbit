using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class AcosLambdaExpression : SystemLambdaExpression
    {
        public AcosLambdaExpression()
            : base(typeof(Math).GetMethod("Acos", new Type[] { typeof(double) }), "acos", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override double Eval(RuntimeContext context)
            {
                return Math.Acos(ParameterExpression.Access(context, "x"));
            }
        }
    }
}