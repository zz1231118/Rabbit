using IronRabbit.Runtime;

namespace IronRabbit.Expressions
{
    public class MemberExpression : Expression
    {
        internal MemberExpression(Expression instance, string name)
        {
            Instance = instance;
            Name = name;
        }

        public override ExpressionType NodeType => ExpressionType.MemberAccess;

        public override Type Type => typeof(decimal);

        public Expression Instance { get; }

        public string Name { get; }

        public override object Eval(RuntimeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return default(decimal);
        }

        public override string ToString()
        {
            return Instance == null ? Name : Instance.ToString() + "." + Name;
        }
    }
}
