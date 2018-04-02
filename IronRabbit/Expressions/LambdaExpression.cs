using System;
using System.Collections.ObjectModel;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class LambdaExpression : Expression
    {
        internal LambdaExpression(string name, Expression body, params ParameterExpression[] parameters)
            : base(ExpressionType.Lambda)
        {
            Name = name;
            Parameters = new ReadOnlyCollection<ParameterExpression>(parameters);
            Body = body;
        }

        public RabbitDomain Domain { get; internal set; }
        public string Name { get; }
        public ReadOnlyCollection<ParameterExpression> Parameters { get; }
        public Expression Body { get; }

        public Delegate Compile(Type deletageType = null)
        {
            var lambdaCompiler = new LambdaCompiler(this);
            return lambdaCompiler.Compile(deletageType);
        }
        public T Compile<T>()
        {
            return (T)(object)Compile(typeof(T));
        }
        public override double Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var lambdaContext = new RuntimeContext(context);
            lambdaContext.Domain = Domain;
            for (int i = 0; i < Parameters.Count; i++)
            {
                var parameter = Parameters[i];
                var value = parameter.Eval(context);
                lambdaContext.Variable(parameter.Name, value);
            }

            return Body.Eval(lambdaContext);
        }
    }
}
