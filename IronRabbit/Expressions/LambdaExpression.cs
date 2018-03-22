using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class LambdaExpression : Expression
    {
        internal LambdaExpression(string name, IList<ParameterExpression> parameters, Expression body)
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

        public override decimal Eval(RuntimeContext context)
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
