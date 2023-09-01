using System.Collections.ObjectModel;
using System.Text;
using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class LambdaExpression : Expression
    {
        internal LambdaExpression(string name, Expression body, params ParameterExpression[] parameters)
        {
            Name = name;
            Parameters = new ReadOnlyCollection<ParameterExpression>(parameters);
            Body = body;
        }

        public override ExpressionType NodeType => ExpressionType.Lambda;

        public override Type Type => typeof(decimal);

        public RabbitDomain? Domain { get; internal set; }

        public string Name { get; }

        public ReadOnlyCollection<ParameterExpression> Parameters { get; }

        public Expression Body { get; }

        public Delegate Compile(Type? deletageType = null)
        {
            var lambdaCompiler = new ExpressionCompiler();
            return lambdaCompiler.Compile(this, deletageType);
        }

        public T Compile<T>()
            where T : Delegate
        {
            return (T)Compile(typeof(T));
        }

        public override object Eval(RuntimeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var lambdaContext = new RuntimeContext(context);
            lambdaContext.Domain = Domain;
            for (int i = 0; i < Parameters.Count; i++)
            {
                var parameter = Parameters[i];
                var value = parameter.Eval(context);
                lambdaContext.Define(parameter.Name, value);
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
