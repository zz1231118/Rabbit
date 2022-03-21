using System;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class MemberExpression : Expression
    {
        internal MemberExpression(Expression instance, string name)
            : base(ExpressionType.MemberAccess)
        {
            Instance = instance;
            Name = name;
        }

        public Expression Instance { get; }

        public string Name { get; }

        public override object Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return default(double);
        }

        public override string ToString()
        {
            return Instance == null ? Name : Instance.ToString() + "." + Name;
        }
    }
}
