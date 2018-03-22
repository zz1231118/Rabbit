using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("round", "d", "decimals")]
    internal class RoundLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            decimal d = ParameterExpression.Access(context, "d");
            decimal decimals = ParameterExpression.Access(context, "decimals");
            return (decimal)Math.Round((double)d, (int)decimals);
        }
    }
}
