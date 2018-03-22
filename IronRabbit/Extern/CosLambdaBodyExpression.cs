using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("cos", "x")]
    internal class CosLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            return (decimal)Math.Cos((double)ParameterExpression.Access(context, "x"));
        }
    }
}
