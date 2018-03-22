using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IronRabbit.Expressions;
using IronRabbit.Extern;

namespace IronRabbit
{
    internal class Rabbit
    {
        private static Dictionary<string, LambdaExpression> _systemFunctions = new Dictionary<string, LambdaExpression>();

        static Rabbit()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(ExternMethodAttribute), false);
                if (attributes.Length > 0)
                {
                    var @extern = attributes.First() as ExternMethodAttribute;
                    ParameterExpression[] parameters = new ParameterExpression[@extern.Parameters.Count];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = Expression.Parameter(@extern.Parameters[i]);
                    }

                    Expression body = Activator.CreateInstance(type) as Expression;
                    Register(Expression.Lambda(@extern.EntryPoint, body, parameters));
                }
            }
        }

        internal static void Register(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException(nameof(lambda));

            _systemFunctions[lambda.Name] = lambda;
        }
        internal static bool TryGetSystemLambda(string name, out LambdaExpression lambda)
        {
            return _systemFunctions.TryGetValue(name, out lambda);
        }
        internal static LambdaExpression GetSystemLambda(string name)
        {
            LambdaExpression lambda;
            _systemFunctions.TryGetValue(name, out lambda);
            return lambda;
        }
    }
}
