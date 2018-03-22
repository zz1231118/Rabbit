using System;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class ParameterExpression : Expression
    {
        internal ParameterExpression(ExpressionType type, string name)
            : base(type)
        {
            Name = name;
        }

        public string Name { get; }

        internal static decimal Access(RuntimeContext context, string name)
        {
            var value = context.Access(name);
            if (value == null)
                throw new IronRabbit.Runtime.MissingMemberException(string.Format("missing member:{0}", name));

            return value.Value;
        }

        public override decimal Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return Access(context, Name);
        }
    }
}
