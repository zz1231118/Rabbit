using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("floor", "x")]
    internal class FloorLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            return Math.Floor(ParameterExpression.Access(context, "x"));
        }
    }
}