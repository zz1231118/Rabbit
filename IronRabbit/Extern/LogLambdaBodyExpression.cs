using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("log", "", "x", "e")]
    internal class LogLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            decimal x = ParameterExpression.Access(context, "x");
            decimal e = ParameterExpression.Access(context, "e");
            return (decimal)Math.Log((double)x, (double)e);
        }
    }
}
