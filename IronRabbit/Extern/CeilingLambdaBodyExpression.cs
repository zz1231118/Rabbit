using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("ceiling", "x")]
    internal class CeilingLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            return Math.Ceiling(ParameterExpression.Access(context, "x"));
        }
    }
}
