using System;
using System.Collections.Generic;
using System.IO;
using IronRabbit.Compiler;
using IronRabbit.Expressions;

namespace IronRabbit
{
    public class RabbitDomain
    {
        private Dictionary<string, LambdaExpression> _functions = new Dictionary<string, LambdaExpression>();

        public void Register(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException(nameof(lambda));

            _functions[lambda.Name] = lambda;
        }
        public bool TryGetLambda(string name, out LambdaExpression lambda)
        {
            if (_functions.TryGetValue(name, out lambda))
            {
                return true;
            }

            return Rabbit.TryGetSystemLambda(name, out lambda);
        }
        public LambdaExpression GetLambda(string name)
        {
            LambdaExpression lambda;
            TryGetLambda(name, out lambda);
            return lambda;
        }

        public LambdaExpression CompileFromSource(string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (var reader = new StringReader(source))
            {
                var tokenizer = new Tokenizer(reader);
                var parser = new Parser(tokenizer);
                var lambda = parser.Parse();
                lambda.Domain = this;
                Register(lambda);
                return lambda;
            }
        }
        public LambdaExpression CompileFromFile(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    var tokenizer = new Tokenizer(reader);
                    var parser = new Parser(tokenizer);
                    var lambda = parser.Parse();
                    lambda.Domain = this;
                    Register(lambda);
                    return lambda;
                }
            }
        }
    }
}
