using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class AtanLambdaExpression : SystemLambdaExpression
    {
        public AtanLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Atan), new Type[] { typeof(double) }), "atan", new BodyExpression(), 
                  Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Atan(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}