using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class RoundLambdaExpression : SystemLambdaExpression
    {
        public RoundLambdaExpression()
            : base(typeof(Math).GetMethod("Round", new Type[] { typeof(double), typeof(int) }), "round", new BodyExpression(), Expression.Parameter(typeof(double), "x"), Expression.Parameter(typeof(double), "decimals"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                double d = ParameterExpression.Access<double>(context, "d");
                double decimals = ParameterExpression.Access<double>(context, "decimals");
                return Math.Round(d, (int)decimals);
            }
        }
    }
}
