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
        private static Dictionary<string, SystemLambdaExpression> _systemFunctions = new Dictionary<string, SystemLambdaExpression>();

        static Rabbit()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(ExternLambdaAttribute), false);
                if (attributes.Length > 0)
                {
                    var constructor = type.GetConstructor(Type.EmptyTypes);
                    Register(constructor.Invoke(null) as SystemLambdaExpression);
                }
            }
        }

        internal static void Register(SystemLambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException(nameof(lambda));

            _systemFunctions[lambda.Name] = lambda;
        }
        internal static bool TryGetSystemLambda(string name, out SystemLambdaExpression lambda)
        {
            return _systemFunctions.TryGetValue(name, out lambda);
        }
        internal static SystemLambdaExpression GetSystemLambda(string name)
        {
            SystemLambdaExpression lambda;
            _systemFunctions.TryGetValue(name, out lambda);
            return lambda;
        }
    }
}
