using System;
using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Extern
{
    [ExternMethod("abs", "x")]
    internal class AbsLambdaBodyExpression : Expression
    {
        public override decimal Eval(RuntimeContext context)
        {
            return Math.Abs(ParameterExpression.Access(context, "x"));
        }
    }
}