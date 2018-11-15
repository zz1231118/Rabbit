using System;
using System.Collections.ObjectModel;
using System.Text;
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
        public override object Eval(RuntimeContext context)
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
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Name).Append('(');
            using (var e = Parameters.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    sb.Append(e.Current.Name);
                    while (e.MoveNext())
                    {
                        sb.Append(',');
                        sb.Append(e.Current.Name);
                    }
                }
            }

            sb.Append(")=").Append(Body.ToString());
            return sb.ToString();
        }
    }
}
