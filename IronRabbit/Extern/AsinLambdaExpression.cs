using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class AsinLambdaExpression : SystemLambdaExpression
    {
        public AsinLambdaExpression()
            : base(typeof(Math).GetMethod("Asin", new Type[] { typeof(double) }), "asin", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }        

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Asin(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}