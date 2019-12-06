using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class CoshLambdaExpression : SystemLambdaExpression
    {
        public CoshLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Cosh), new Type[] { typeof(double) }), "cosh", new BodyExpression(), 
                  Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Cosh(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}
