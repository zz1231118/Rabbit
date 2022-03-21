using System;
using System.Collections.Generic;
using System.IO;
using IronRabbit.Compiler;
using IronRabbit.Expressions;

namespace IronRabbit
{
    public class RabbitDomain
    {
        private readonly Dictionary<string, LambdaExpression> lambdas = new Dictionary<string, LambdaExpression>();

        public void Register(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException(nameof(lambda));

            lambdas[lambda.Name] = lambda;
        }

        public bool TryGetLambda(string name, out LambdaExpression lambda)
        {
            if (lambdas.TryGetValue(name, out lambda))
            {
                return true;
            }
            if (Rabbit.TryGetSystemLambda(name, out var systemLambda))
            {
                lambda = systemLambda;
                return true;
            }

            return false;
        }

        public LambdaExpression GetLambda(string name)
        {
            TryGetLambda(name, out LambdaExpression lambda);
            return lambda;
        }

        public LambdaExpression CompileFromSource(string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            LambdaExpression lambda = null;
            using var reader = new StringReader(source);
            var tokenizer = new Tokenizer(reader);
            var parser = new Parser(tokenizer);
            while (!tokenizer.EndOfStream)
            {
                var expression = parser.Parse();
                if (expression == null) break;

                expression.Domain = this;
                Register(expression);
                lambda = expression;
            }

            return lambda ?? throw new CompilerException(tokenizer.Position, "The formula was not found");
        }

        public LambdaExpression CompileFromFile(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            LambdaExpression lambda = null;
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            var tokenizer = new Tokenizer(reader);
            var parser = new Parser(tokenizer);
            while (!tokenizer.EndOfStream)
            {
                var expression = parser.Parse();
                if (expression == null) break;

                expression.Domain = this;
                Register(expression);
                lambda = expression;
            }

            return lambda ?? throw new CompilerException(tokenizer.Position, "The formula was not found");
        }
    }
}
