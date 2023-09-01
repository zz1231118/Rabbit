using System.Collections.ObjectModel;
using System.Text;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class MethodCallExpression : Expression
    {
        internal MethodCallExpression(Expression? instance, string methodName, IList<Expression> arguments)
        {
            Instance = instance;
            MethodName = methodName;
            Arguments = new ReadOnlyCollection<Expression>(arguments);
        }

        public override ExpressionType NodeType => ExpressionType.MethodCall;

        public override Type Type => typeof(decimal);

        public Expression? Instance { get; }

        public string MethodName { get; }

        public ReadOnlyCollection<Expression> Arguments { get; }

        internal LambdaExpression? GetLambda(RabbitDomain? domain)
        {
            LambdaExpression? lambda;
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

        public override object Eval(RuntimeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (Instance == null)
            {
                var lambdaExpression = GetLambda(context.Domain);
                if (lambdaExpression == null)
                    throw new MissingMethodException(string.Format("missing method:{0}", MethodName));
                if (Arguments.Count != lambdaExpression.Parameters.Count)
                    throw new RuntimeException(string.Format("method:{0}. parame count error!", MethodName));

                var lambdaContext = new RuntimeContext(context);
                for (int i = 0; i < Arguments.Count; i++)
                {
                    var parameter = lambdaExpression.Parameters[i];
                    var argument = Arguments[i];
                    var value = argument.Eval(context);
                    lambdaContext.Define(parameter.Name, value);
                }

                return lambdaExpression.Body.Eval(lambdaContext);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(MethodName).Append('(');
            using (var e = Arguments.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    sb.Append(e.Current.ToString());
                    while (e.MoveNext())
                    {
                        sb.Append(',');
                        sb.Append(e.Current.ToString());
                    }
                }
            }
            sb.Append(')');
            return sb.ToString();
        }
    }
}
