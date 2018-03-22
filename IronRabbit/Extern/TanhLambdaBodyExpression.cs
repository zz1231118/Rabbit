using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("tanh", "x")]
    internal class TanhLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            return (decimal)Math.Tanh((double)ParameterExpression.Access(context, "x"));
        }
    }
}
