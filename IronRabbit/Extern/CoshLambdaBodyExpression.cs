using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("cosh", "x")]
    internal class CoshLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            return (decimal)Math.Cosh((double)ParameterExpression.Access(context, "x"));
        }
    }
}
