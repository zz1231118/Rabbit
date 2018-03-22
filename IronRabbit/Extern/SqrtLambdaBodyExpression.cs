using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("sqrt", "x")]
    internal class SqrtLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            return (decimal)Math.Sqrt((double)ParameterExpression.Access(context, "x"));
        }
    }
}
