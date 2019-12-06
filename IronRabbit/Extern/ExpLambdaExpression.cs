using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class ExpLambdaExpression : SystemLambdaExpression
    {
        public ExpLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Exp), new Type[] { typeof(double) }), "exp", new BodyExpression(), 
                  Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Exp(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}
