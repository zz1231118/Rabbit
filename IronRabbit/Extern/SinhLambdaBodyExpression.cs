using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("sinh", "x")]
    internal class SinhLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            return (decimal)Math.Sinh((double)ParameterExpression.Access(context, "x"));
        }
    }
}
