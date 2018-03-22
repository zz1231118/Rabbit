using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("sin", "x")]
    internal class SinLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            return (decimal)Math.Sin((double)ParameterExpression.Access(context, "x"));
        }
    }
}
