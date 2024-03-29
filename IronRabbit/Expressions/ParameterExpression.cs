﻿using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class ParameterExpression : Expression
    {
        internal ParameterExpression(ExpressionType nodeType, Type type, string name)
        {
            Type = type;
            Name = name;
            NodeType = nodeType;
        }

        public override ExpressionType NodeType { get; }

        public override Type Type { get; }

        public string Name { get; }

        internal static object Access(RuntimeContext context, string name)
        {
            var value = context.Access(name);
            if (value == null) throw new MissingMemberException(string.Format("missing member:{0}", name));

            return value.Value;
        }

        internal static T Access<T>(RuntimeContext context, string name)
        {
            return (T)Access(context, name);
        }

        public override object Eval(RuntimeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return Access(context, Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
