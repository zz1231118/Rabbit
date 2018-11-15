using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternLambda]
    internal class FloorLambdaExpression : SystemLambdaExpression
    {
        public FloorLambdaExpression()
            : base(typeof(Math).GetMethod("Floor", new Type[] { typeof(double) }), "floor", new BodyExpression(), Expression.Parameter(typeof(double), "x"))
        { }

        class BodyExpression : Expression
        {
            public override object Eval(RuntimeContext context)
            {
                return Math.Floor(ParameterExpression.Access<double>(context, "x"));
            }
        }
    }
}