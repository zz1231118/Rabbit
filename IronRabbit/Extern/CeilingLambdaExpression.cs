using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class CeilingLambdaExpression : SystemLambdaExpression
    {
        public CeilingLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Ceiling), new Type[] { typeof(double) }), "ceiling", new BodyExpression(), 
                  Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Ceiling(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}
