using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class MethodCallExpression : Expression
    {
        internal MethodCallExpression(Expression instance, string methodName, IList<Expression> arguments)
            : base(ExpressionType.MethodCall)
        {
            Object = instance;
            MethodName = methodName;
            Arguments = new ReadOnlyCollection<Expression>(arguments);
        }

        public Expression Object { get; }
        public string MethodName { get; }
        public ReadOnlyCollection<Expression> Arguments { get; }

        internal LambdaExpression GetLambda(RabbitDomain domain)
        {
            LambdaExpression lambda;
            if (domain != null)
            {
                lambda = domain.GetLambda(MethodName);
            }
            else
            {
                lambda = Rabbit.GetSystemLambda(MethodName);
            }

            return lambda;
        }

        public override double Eval(RuntimeContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (Object == null)
            {
                var lambdaExpression = GetLambda(context.Domain);
                if (lambdaExpression == null)
                    throw new IronRabbit.Runtime.MissingMethodException(string.Format("missing method:{0}", MethodName));
                if (Arguments.Count != lambdaExpression.Parameters.Count)
                    throw new RuntimeException(string.Format("method:{0}. parame count error!", MethodName));

                var lambdaContext = new RuntimeContext(context);
                for (int i = 0; i < Arguments.Count; i++)
                {
                    var parameter = lambdaExpression.Parameters[i];
                    var argument = Arguments[i];
                    var value = argument.Eval(context);
                    lambdaContext.Variable(parameter.Name, value);
                }

                return lambdaExpression.Body.Eval(lambdaContext);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
