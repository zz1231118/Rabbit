using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class RoundLambdaExpression : SystemLambdaExpression
    {
        public RoundLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Round), new Type[] { typeof(double), typeof(int) }), "round", new BodyExpression(), 
                  Expression.Parameter(typeof(double), "value"), 
                  Expression.Parameter(typeof(double), "digits"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                var value = ParameterExpression.Access<double>(context, "value");
                var digits = (int)ParameterExpression.Access<double>(context, "digits");
                return Math.Round(value, digits);
            }
        }
    }
}
