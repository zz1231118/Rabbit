using System;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class MemberExpression : Expression
    {
        internal MemberExpression(Expression instance, string memberName)
            : base(ExpressionType.MemberAccess)
        {
            Object = instance;
            MemberName = memberName;
        }

        public Expression Object { get; }
        public string MemberName { get; }

        public override double Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return default(double);
        }
    }
}
