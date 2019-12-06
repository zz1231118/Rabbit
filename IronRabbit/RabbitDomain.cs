using System;
using System.Collections.Generic;
using IronRabbit.Expressions;

namespace IronRabbit
{
    public class RabbitDomain
    {
        private Dictionary<string, LambdaExpression> functions = new Dictionary<string, LambdaExpression>();

        public void Register(LambdaExpression lambda)
        {
            if (lambda == null)
                throw new ArgumentNullException(nameof(lambda));

            functions[lambda.Name] = lambda;
        }
        public bool TryGetLambda(string name, out LambdaExpression lambda)
        {
            if (functions.TryGetValue(name, out lambda))
            {
                return true;
            }
            if (Rabbit.TryGetSystemLambda(name, out SystemLambdaExpression systemLambda))
            {
                lambda = systemLambda;
                return true;
            }

            return false;
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

            var lambda = Rabbit.CompileFromSource(source);
            lambda.Domain = this;
            Register(lambda);
            return lambda;
        }
        public LambdaExpression CompileFromFile(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var lambda = Rabbit.CompileFromFile(path);
            lambda.Domain = this;
            Register(lambda);
            return lambda;
        }
    }
}
