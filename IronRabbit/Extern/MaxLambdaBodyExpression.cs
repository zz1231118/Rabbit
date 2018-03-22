using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("max", "lh", "rh")]
    internal class MaxLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            decimal lh = ParameterExpression.Access(context, "lh");
            decimal rh = ParameterExpression.Access(context, "rh");
            return Math.Max(lh, rh);
        }
    }
}
