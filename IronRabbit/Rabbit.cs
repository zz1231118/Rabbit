using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IronRabbit.Compiler;
using IronRabbit.Expressions;
using IronRabbit.Extern;

namespace IronRabbit
{
    public class Rabbit
    {
        private static Dictionary<string, SystemLambdaExpression> systemFunctions = new Dictionary<string, SystemLambdaExpression>();

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

            systemFunctions[lambda.Name] = lambda;
        }
        internal static bool TryGetSystemLambda(string name, out SystemLambdaExpression lambda)
        {
            return systemFunctions.TryGetValue(name, out lambda);
        }
        internal static SystemLambdaExpression GetSystemLambda(string name)
        {
            SystemLambdaExpression lambda;
            systemFunctions.TryGetValue(name, out lambda);
            return lambda;
        }

        public static LambdaExpression CompileFromSource(string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (var reader = new StringReader(source))
            {
                var tokenizer = new Tokenizer(reader);
                var parser = new Parser(tokenizer);
                return parser.Parse();
            }
        }
        public static LambdaExpression CompileFromFile(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    var tokenizer = new Tokenizer(reader);
                    var parser = new Parser(tokenizer);
                    return parser.Parse();
                }
            }
        }
    }
}
