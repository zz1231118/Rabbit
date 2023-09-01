using IronRabbit.Expressions;
using IronRabbit.Runtime;

namespace IronRabbit.Builtin
{
    internal sealed class AcosLambdaExpression : SystemLambdaExpression
    {
        public AcosLambdaExpression()
            : base(typeof(Math).GetMethod(nameof(Math.Acos), new Type[] { typeof(double) })!, "acos", new BodyExpression(),
                  Expression.Parameter(typeof(decimal), "x"))
        { }

        sealed class BodyExpression : SystemLambdaBodyExpression
        {
            public override object Eval(RuntimeContext context)
            {
                return checked((decimal)Math.Acos((double)ParameterExpression.Access<decimal>(context, "x")));
            }
        }
    }
}