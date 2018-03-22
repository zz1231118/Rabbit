using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("min", "lh", "rh")]
    internal class MinLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            decimal lh = ParameterExpression.Access(context, "lh");
            decimal rh = ParameterExpression.Access(context, "rh");
            return Math.Min(lh, rh);
        }
    }
}
