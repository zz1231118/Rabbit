using System;
using System.Reflection;

namespace IronRabbit.Expressions
{
    public abstract class SystemLambdaExpression : LambdaExpression
    {
        internal SystemLambdaExpression(MethodInfo method, string name, Expression body, params ParameterExpression[] parameters)
            : base(name, body, parameters)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            Method = method;
        }

        public MethodInfo Method { get; }
    }
}
